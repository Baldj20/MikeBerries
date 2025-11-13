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

public class ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger) : IProductService
{
    public async Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token)
    {
        var product = productModel.Adapt<Product>();

        await unitOfWork.Products.AddAsync(product, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.ResourceAdded(typeof(Product).Name, product.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            await unitOfWork.Products.Delete(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.ResourceDeleted(typeof(Product).Name, product.Id);

            return Result.Success();
        }
        else
        {
            logger.ResourceToDeleteNotFound(typeof(Product).Name);

            return Result
                .Failure(CustomError.ResourceNotFound<Product>());
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            logger.ResourceReturned(typeof(Product).Name, product.Id);

            return new Result<ProductModel>(product.Adapt<ProductModel>());
        }
        else
        {
            logger.ResourceNotFound(typeof(Product).Name, id);

            return new Result<ProductModel>(CustomError.ResourceNotFound<Product>());
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
                logger.ResourceReturned(typeof(Product).Name, item.Id);
            }

            return new Result<List<ProductModel>>(result.Adapt<List<ProductModel>>());
        }
        else
        {
            logger.FilteredResourcesNotFound(typeof(Product).Name);

            return new Result<List<ProductModel>>(CustomError
                .ResourceNotFound<Product>());
        }
    }

    public async Task<Result> UpdateProductAsync(Guid id, ProductModel productModel, CancellationToken token)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            logger.ResourceToUpdateNotFound(typeof(Product).Name);

            return Result.Failure(CustomError.ResourceNotFound<Product>());
        }
        else
        {
            productModel.Adapt(product);

            await unitOfWork.SaveChangesAsync(token);

            logger.ResourceUpdated(typeof(Product).Name, product.Id);

            return Result.Success();
        }       
    }
}
