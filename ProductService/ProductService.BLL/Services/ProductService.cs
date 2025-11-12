using Mapster;
using Microsoft.Extensions.Logging;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.BLL.Services;

public class ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger) : IProductService
{
    public async Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token)
    {
        var product = productModel.Adapt<Product>();

        await unitOfWork.Products.AddAsync(product, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.LogInformation(LoggingConstants.RESOURCE_ADDED, 
            typeof(Product).Name, 
            product.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            await unitOfWork.Products.Delete(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants.RESOURCE_DELETED, 
                typeof(Product).Name, 
                id);

            return Result.Success();
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCE_TO_DELETE_NOT_FOUND, 
                typeof(Product).Name);

            var errorMessage = LoggingConstants.RESOURCE_TO_DELETE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Product).Name);

            return Result
                .Failure(CustomError.ResourceNotFound(errorMessage));
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            logger.LogInformation(LoggingConstants.RESOURCE_RETURNED, 
                typeof(Product).Name, 
                id);

            return new Result<ProductModel>(product.Adapt<ProductModel>());
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCE_NOT_FOUND, typeof(Product).Name, id);

            var errorMessage = LoggingConstants.RESOURCE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Product).Name);

            errorMessage = errorMessage
                .Replace("{ResourceId}", id.ToString());

            return new Result<ProductModel>(CustomError.ResourceNotFound(errorMessage));
        }
    }

    public Result<List<ProductModel>> GetProducts(PaginationParams paginationParams, 
        ProductFilter filter, CancellationToken token)
    {
        var result = unitOfWork.Products.GetPaged(paginationParams, filter);

        if (result.Count != 0)
        {
            foreach (var item in result)
            {
                logger.LogInformation(LoggingConstants.RESOURCE_RETURNED, 
                    typeof(Product).Name, 
                    item.Id);
            }

            return new Result<List<ProductModel>>(result.Adapt<List<ProductModel>>());
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCES_FILTERED_NOT_FOUND,
                typeof(Product).Name);

            var errorMessage = LoggingConstants.RESOURCES_FILTERED_NOT_FOUND
                .Replace("{ResourceName}", typeof(Product).Name);

            return new Result<List<ProductModel>>(CustomError
                .ResourceNotFound(errorMessage));
        }
    }

    public async Task<Result> UpdateProductAsync(Guid id, ProductModel productModel, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            logger.LogWarning(LoggingConstants.RESOURCE_TO_UPDATE_NOT_FOUND, 
                typeof(Product).Name);

            var errorMessage = LoggingConstants.RESOURCE_TO_UPDATE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Product).Name);

            return Result.Failure(CustomError.ResourceNotFound(errorMessage));
        }
        else
        {
            productModel.Adapt(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants.RESOURCE_UPDATED, 
                typeof(Product).Name, 
                id);

            return Result.Success();
        }       
    }
}
