using Mapster;
using ProductService.BLL.Configurations;
using ProductService.BLL.DTO;
using ProductService.BLL.Models;
using ProductService.DAL.Entities;

namespace ProductService.API.Configurations;

public static class APILayerExtention
{
    public static void ConfigureMapping()
    {
        TypeAdapterConfig<Provider, ProviderDTO>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductDTO, ProductModel>.NewConfig();
        TypeAdapterConfig<ProductModel, GetProductDTO>.NewConfig();
        TypeAdapterConfig<UpdateProductDTO, ProductModel>.NewConfig();

        TypeAdapterConfig<ProductImageModel, GetProductImageDTO>.NewConfig();
        TypeAdapterConfig<UploadProductImageDTO, ProductImageModel>.NewConfig();

        BusinessLogicLayerExtension.ConfigureMapping();
    }
}
