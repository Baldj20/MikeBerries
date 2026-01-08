using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Services;

namespace ProductService.BLL.Configurations;

public static class BusinessLogicLayerExtension
{
    public static void ConfigureBusinessLogicLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductService, Services.ProductService>();
        services.AddScoped<IProviderService, ProviderService>();
    }
}
