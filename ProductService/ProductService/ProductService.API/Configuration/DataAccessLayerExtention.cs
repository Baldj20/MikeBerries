using Microsoft.EntityFrameworkCore;
using ProductService.DAL;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

namespace ProductService.API.Configuration;

public static class DataAccessLayerExtention
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MikeBerriesDBContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    public static void ConfigureDIContainer(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
