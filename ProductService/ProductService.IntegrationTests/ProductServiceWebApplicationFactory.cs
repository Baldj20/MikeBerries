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
            services.RemoveAll<DbContextOptions<MikeBerriesDBContext>>();
            services.RemoveAll<MikeBerriesDBContext>();

            services.RemoveAll<IFileRepository>();
            services.RemoveAll<MinioStorage>();

            services.AddDbContext<MikeBerriesDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });

            services.AddScoped<IFileRepository>(provider =>
            {
                return new FakeFileRepository();
            });
        });
    }
}

