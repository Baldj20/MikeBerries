using Mapster;
using Microsoft.Extensions.Logging;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.BLL.Services;

public class ProductService(IUnitOfWork unitOfWork, ICacheRepository cache,
    ILogger<ProductService> logger) : IProductService
{
    public async Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token)
    {
        var product = productModel.Adapt<Product>();
        var imageModels = productModel.Images;

        for (int i = 0; i < imageModels.Count; i++)
        {
            var key = $"products/{product.Id}/{product.Images[i].Id}";
            using var fileStream = imageModels[i].Image!.OpenReadStream();
            var url = await unitOfWork.Files.UploadFileAsync(key, fileStream, token);
            product.Images[i].Url = url;
        }

        await unitOfWork.Products.AddAsync(product, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.ResourceAdded(typeof(Product).Name, product.Id);

        return Result.Success(201);
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            await unitOfWork.Products.Delete(product);

            await unitOfWork.SaveChangesAsync(token);

            var cacheKey = $"product:{id}";
            await cache.RemoveData(cacheKey, token);

            logger.ResourceDeleted(typeof(Product).Name, product.Id);

            return Result.Success(204);
        }
        else
        {
            logger.ResourceToDeleteNotFound(typeof(Product).Name);

            return Result
                .Failure(CustomError.ResourceNotFound<Product>(), 204);
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var cacheKey = $"product:{id}";
        var productModel = await cache.GetData<ProductModel>(cacheKey, token);

        if (productModel is not null)
        {
            return new Result<ProductModel>(productModel, 200);
        }

        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            logger.ResourceReturned(typeof(Product).Name, product.Id);

            var model = product.Adapt<ProductModel>();

            await cache.SetData(cacheKey, model);

            return new Result<ProductModel>(model, 200);
        }
        else
        {
            logger.ResourceNotFound(typeof(Product).Name, id);

            return new Result<ProductModel>(CustomError.ResourceNotFound<Product>(), 404);
        }
    }

    public Result<PagedResult<ProductModel>> GetProducts(PaginationParams paginationParams, 
        ProductFilter filter, CancellationToken token)
    {
        var result = unitOfWork.Products.GetPaged(paginationParams, filter);

        if (result.Items.Count != 0)
        {
            foreach (var item in result.Items)
            {
                logger.ResourceReturned(typeof(Product).Name, item.Id);
            }

            return new Result<PagedResult<ProductModel>>(result.Adapt<PagedResult<ProductModel>>(), 200);
        }
        else
        {
            logger.FilteredResourcesNotFound(typeof(Product).Name);

            return new Result<PagedResult<ProductModel>>(CustomError
                .ResourceNotFound<Product>(), 404);
        }
    }

    public async Task<Result> UpdateProductAsync(Guid id, UpdateProductModel productModel, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            logger.ResourceToUpdateNotFound(typeof(Product).Name);

            return Result.Failure(CustomError.ResourceNotFound<Product>(), 404);
        }
        else
        {
            productModel.Adapt(product);

            foreach (var item in productModel.Images)
            {
                if (item.Action is UpdateImageAction.Delete)
                {
                    var uri = new Uri(item.Url!);
                    var cleanPath = uri.AbsolutePath.TrimStart('/');
                    var key = cleanPath.Substring(cleanPath.IndexOf('/') + 1);

                    await unitOfWork.Files.DeleteFileAsync(key, token);

                    var imageEntity = product.Images.FirstOrDefault(img => img.Url == item.Url);
                    if (imageEntity is not null)
                        await unitOfWork.Images.Delete(imageEntity);
                }
                else if (item.Action is UpdateImageAction.Add)
                {
                    var image = new ProductImage
                    {
                        Url = string.Empty,
                        Product = product,
                        ProductId = product.Id
                    };

                    var key = $"products/{product.Id}/{image.Id}";
                    using var fileStream = item.Image!.OpenReadStream();
                    var url = await unitOfWork.Files.UploadFileAsync(key, fileStream, token);
                    image.Url = url;
                    
                    await unitOfWork.Images.AddAsync(image, token);
                }
            }

            await unitOfWork.SaveChangesAsync(token);

            var cacheKey = $"product:{id}";
            await cache.RemoveData(cacheKey, token);

            logger.ResourceUpdated(typeof(Product).Name, product.Id);

            return Result.Success(204);
        }       
    }
}
