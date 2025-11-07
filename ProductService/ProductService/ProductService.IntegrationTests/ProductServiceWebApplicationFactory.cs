using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ProductService.API;
using ProductService.DAL;
using System.Data.Common;

namespace ProductService.IntegrationTests;

public class ProductServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _root = new();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.FirstOrDefault(
                d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<MikeBerriesDBContext>));

            if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.FirstOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));

            if (dbConnectionDescriptor is not null) services.Remove(dbConnectionDescriptor);

            services.AddDbContext<MikeBerriesDBContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase", _root);
            });
        });
    }
}
