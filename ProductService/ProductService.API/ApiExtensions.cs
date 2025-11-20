using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductService.API.DTO;
using ProductService.BLL.DTO;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;

namespace ProductService.API;

public static class ApiExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();

            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(context.Database.Migrate);
        }
    }

    public static void ConfigureMapping()
    {
        TypeAdapterConfig<IFormFile, IFormFile>.NewConfig().MapWith(src => src);
        TypeAdapterConfig<CreateProductDto, ProductModel>.NewConfig()
            .Ignore(dest => dest.Images)
            .AfterMapping((dto, model) =>
            {
                foreach (var image in dto.Images)
                {
                    var imageModel = new ProductImageModel
                    {
                        Image = image,
                        Product = null!
                    };

                    model.Images.Add(imageModel);
                }
            });
        TypeAdapterConfig<Product, ProductModel>.NewConfig()
            .Ignore(d => d.Provider.Products);
        TypeAdapterConfig<UpdateImageDto, UpdateImageModel>.NewConfig();
        TypeAdapterConfig<UpdateProductDto, UpdateProductModel>.NewConfig();
        TypeAdapterConfig<UpdateProductModel, Product>.NewConfig()
            .IgnoreNullValues(true)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Images);

        TypeAdapterConfig<ProductImage, ProductImageModel>.NewConfig()
            .Ignore(d => d.Product.Images)
            .Ignore(d => d.Product.Provider.Products);
        TypeAdapterConfig<ProductImageModel, ProductImage>.NewConfig()
            .Ignore(dest => dest.Product);
    }
}
