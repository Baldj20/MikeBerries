using Microsoft.EntityFrameworkCore;
using ProductService.DAL;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

namespace ProductService.API.Configuration;

public static class DataAccessLayerExtention
{
    public static void ConfigureDataAccessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        services.AddDbContext<MikeBerriesDBContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
