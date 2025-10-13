using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.DAL.Entities;

namespace ProductService.BLL.Configurations;

public static class BusinessLogicLayerExtension
{
    public static void ConfigureBusinessLogicLayerDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductService, Services.ProductService>();

        TypeAdapterConfig<UpdateProductDTO, Product>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ProductId);
    }

    public static void Update(this Product product, UpdateProductDTO dto)
    {
        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Provider = dto.Adapt<Provider>();
        product.Images = dto.Adapt<List<ProductImage>>();
        product.Price = dto.Price;
    }
}
