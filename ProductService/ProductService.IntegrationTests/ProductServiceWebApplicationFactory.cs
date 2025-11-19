using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.API;
using ProductService.DAL;

namespace ProductService.IntegrationTests;

public class ProductServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var testPath = AppContext.BaseDirectory;
            var configPath = Path.Combine(testPath, "appsettings.IntegrationTests.json");
            config.Sources.Clear();
            config.AddJsonFile(configPath, optional: false, reloadOnChange: false);
        });

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
        });
    }
}

