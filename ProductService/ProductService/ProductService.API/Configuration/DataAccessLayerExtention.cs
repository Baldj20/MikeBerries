using Microsoft.EntityFrameworkCore;
using ProductService.DAL;

namespace ProductService.API.Configuration;

public static class DataAccessLayerExtention
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MikeBerriesDBContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}
