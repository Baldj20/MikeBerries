using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ProductService.API;
using ProductService.DAL;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.IntegrationTests;

public class ProductServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTesting");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<MikeBerriesDBContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            var genericDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions));

            if (genericDescriptor is not null)
                services.Remove(genericDescriptor);

            var contextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(MikeBerriesDBContext));

            if (contextDescriptor is not null)
                services.Remove(contextDescriptor);

            services.RemoveAll<IFileRepository>();

            services.AddDbContext<MikeBerriesDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });

            services.AddScoped<IFileRepository, FakeFileRepository>();
        });
    }
}

