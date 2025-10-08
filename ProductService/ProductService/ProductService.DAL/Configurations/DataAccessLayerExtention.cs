using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.DAL;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

namespace ProductService.DAL.Configurations;

public static class DataAccessLayerExtention
{
    public static void ConfigureDataAccessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MikeBerriesDBContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
