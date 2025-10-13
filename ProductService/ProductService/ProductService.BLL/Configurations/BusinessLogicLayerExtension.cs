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

    public static void Update(this Product product, ProductModel model)
    {
        product.Title = model.Title;
        product.Description = model.Description;
        product.Provider = model.Adapt<Provider>();
        product.Images = model.Adapt<List<ProductImage>>();
        product.Price = model.Price;
    }
    public static void Update(this Provider provider, ProviderModel model)
    {
        provider.Email = model.Email;
        provider.Name = model.Name;
        provider.Products = model.Products;
    }
}
