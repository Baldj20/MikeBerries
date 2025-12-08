using Microsoft.AspNetCore.Authentication;
﻿using Medallion.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Polly.Registry;
using ProductService.API;
using ProductService.DAL;
using ProductService.IntegrationTests.Fakes;

namespace ProductService.IntegrationTests;

public class ProductServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureServices(services =>
        {
            var descriptorsToRemove = services.Where(d =>
                d.ServiceType.Name.Contains("DbContextOptions")
            ).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<MikeBerriesDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });

            services.RemoveAll<IDistributedLockProvider>();
            services.AddSingleton<IDistributedLockProvider, FakeDistributedLockProvider>();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ResiliencePipelineProvider<string>>();
            services.AddSingleton<ResiliencePipelineProvider<string>, FakeResiliencePipelineProvider>();
        });

        builder.ConfigureTestServices(services =>
        {
            services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "Test", options => { });
        });
    }
}

