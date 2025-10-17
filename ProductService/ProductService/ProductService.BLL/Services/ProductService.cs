using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;
using Serilog;

namespace ProductService.BLL.Services;

public class ProductService(IUnitOfWork unitOfWork, ILogger logger) : IProductService
{
    public async Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token)
    {
        var product = productModel.Adapt<Product>();

        await unitOfWork.Products.AddAsync(product, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.Information(LoggingConstants<Product>.RESOURCE_ADDED, product.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            await unitOfWork.Products.Delete(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.Information(LoggingConstants<Product>.RESOURCE_DELETED, id);

            return Result.Success();
        }
        else
        {
            logger.Warning(LoggingConstants<Product>.RESOURCE_TO_DELETE_NOT_FOUND);

            return Result
                .Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_DELETE_NOT_FOUND));
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            logger.Information(LoggingConstants<Product>.RESOURCE_RETURNED, id);

            return new Result<ProductModel>(product.Adapt<ProductModel>());
        }
        else
        {
            logger.Warning(LoggingConstants<Product>.RESOURCE_NOT_FOUND);

            return new Result<ProductModel>(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_NOT_FOUND));
        }
    }

    public async Task<Result<List<ProductModel>>> GetProductsAsync(PaginationParams paginationParams, 
        ProductFilter filter, CancellationToken token)
    {
        var query = unitOfWork.Products.GetPaged(paginationParams, filter);

        var result = await query.ToListAsync(token);

        if (result.Count != 0)
        {
            foreach (var item in result)
            {
                logger.Information(LoggingConstants<Product>.RESOURCE_RETURNED, item.Id);
            }

            return new Result<List<ProductModel>>(result.Adapt<List<ProductModel>>());
        }
        else
        {
            logger.Warning(LoggingConstants<Product>.RESOURCES_FILTERED_NOT_FOUND);

            return new Result<List<ProductModel>>(CustomError
                .ResourceNotFound(LoggingConstants<Product>.RESOURCES_FILTERED_NOT_FOUND));
        }
    }

    public async Task<Result> UpdateAsync(Guid id, ProductModel productModel, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            logger.Warning(LoggingConstants<Product>.RESOURCE_TO_UPDATE_NOT_FOUND);

            return Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_UPDATE_NOT_FOUND));
        }
        else
        {
            productModel.Adapt(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.Information(LoggingConstants<Product>.RESOURCE_UPDATED, id);

            return Result.Success();
        }       
    }
}
