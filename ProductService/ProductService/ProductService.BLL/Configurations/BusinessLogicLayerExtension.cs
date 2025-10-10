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
}
