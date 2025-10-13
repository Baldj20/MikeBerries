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

    public static void Update(this Product product, ProductModel dto)
    {
        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Provider = dto.Adapt<Provider>();
        product.Images = dto.Adapt<List<ProductImage>>();
        product.Price = dto.Price;
    }
}
