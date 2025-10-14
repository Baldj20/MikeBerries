using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL.Entities;

namespace ProductService.BLL.Configurations;

public static class BusinessLogicLayerExtension
{
    public static void ConfigureBusinessLogicLayerDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Services.ProductService>();
    }

    public static void Update(this Product product, ProductModel model)
    {
        product.Title = model.Title;
        product.Description = model.Description;
        product.Provider = model.Adapt<Provider>();
        product.Images = model.Adapt<List<ProductImage>>();
        product.Price = model.Price;
    }
}
