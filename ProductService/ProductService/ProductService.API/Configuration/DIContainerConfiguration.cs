using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

namespace ProductService.API.Configuration;

public static class DIContainerConfiguration
{
    public static void ConfigureDIContainer(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
    }
}
