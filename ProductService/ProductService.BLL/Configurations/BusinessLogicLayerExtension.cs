using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.BLL.Services;
using ProductService.DAL.Entities;
using Serilog;

namespace ProductService.BLL.Configurations;

public static class BusinessLogicLayerExtension
{
    public static void ConfigureBusinessLogicLayerDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Services.ProductService>();
        services.AddScoped<IProviderService, ProviderService>();

        TypeAdapterConfig<Product, ProductModel>.NewConfig()
            .Ignore(d => d.Provider.Products);
        TypeAdapterConfig<ProductImage, ProductImageModel>.NewConfig()
            .Ignore(d => d.Product.Images)
            .Ignore(d => d.Product.Provider.Products);
    }
}
