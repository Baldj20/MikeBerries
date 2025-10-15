using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.BLL.Services;
using ProductService.DAL.Entities;

namespace ProductService.BLL.Configurations;

public static class BusinessLogicLayerExtension
{
    public static void ConfigureBusinessLogicLayerDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Services.ProductService>();
        services.AddScoped<IProviderService, ProviderService>();
    }

    public static void ConfigureMapping()
    {
        TypeAdapterConfig<ProviderModel, Provider>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductModel, Product>.NewConfig().TwoWays();
        TypeAdapterConfig<ProductImageModel, ProductImage>.NewConfig().TwoWays();
    }
}
