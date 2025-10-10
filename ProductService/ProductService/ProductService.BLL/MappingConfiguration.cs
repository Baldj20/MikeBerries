using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ProductService.BLL.DTO;
using ProductService.DAL.Entities;

namespace ProductService.BLL;

public static class MappingConfiguration
{
    public static void RegisterMappingConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<UpdateProductDTO, Product>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ProductId);
    }
}
