using Mapster;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using ProductService.BLL.Constants.Resilience;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.BLL.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheRepository _cache;
    private readonly ResiliencePipeline _pipeline;
    private readonly ILogger<ProductService> _logger;
    private readonly IDistributedLockProvider _lockProvider;

    public ProductService(IUnitOfWork unitOfWork, 
        ICacheRepository cache,
        ILogger<ProductService> logger,
        IDistributedLockProvider distributedLockProvider,
        ResiliencePipelineProvider<string> pipelineProvider,
        string pipelineName = ResilienceConstants.CACHING_PIPELINE_NAME)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _pipeline = pipelineProvider.GetPipeline(pipelineName);
        _logger = logger;
        _lockProvider = distributedLockProvider;
    }
    public async Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token)
    {
        var product = productModel.Adapt<Product>();
        var imageModels = productModel.Images;

        for (int i = 0; i < imageModels.Count; i++)
        {
            var key = $"products/{product.Id}/{product.Images[i].Id}";
            using var fileStream = imageModels[i].Image!.OpenReadStream();
            var url = await _unitOfWork.Files.UploadFileAsync(key, fileStream, token);
            product.Images[i].Url = url;
        }

        await _unitOfWork.Products.AddAsync(product, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.ResourceAdded(typeof(Product).Name, product.Id);

        return Result.Success(201);
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            _logger.ResourceToDeleteNotFound(typeof(Product).Name);

            return Result
                .Failure(CustomError.ResourceNotFound<Product>(), 204);
        }

        var lockKey = $"lock:product:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            await _unitOfWork.Products.Delete(product);

            await _unitOfWork.SaveChangesAsync(token);

            await _pipeline.ExecuteAsync(async ct =>
            {
                var cacheKey = $"product:{id}";
                await _cache.RemoveData(cacheKey, ct);
            }, token);

            _logger.ResourceDeleted(typeof(Product).Name, product.Id);

            return Result.Success(204);
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var cacheKey = $"product:{id}";

        var productModel = await _pipeline.ExecuteAsync(async ct =>
        {
            return await _cache.GetData<ProductModel>(cacheKey, ct);
        }, token);

        if (productModel is not null)
        {
            return new Result<ProductModel>(productModel, 200);
        }

        var lockKey = $"lock:product:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            productModel = await _pipeline.ExecuteAsync(async ct =>
            {
                return await _cache.GetData<ProductModel>(cacheKey, ct);
            }, token);

            if (productModel is not null)
            {
                return new Result<ProductModel>(productModel, 200);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(id, token);

            if (product is not null)
            {
                _logger.ResourceReturned(typeof(Product).Name, product.Id);

                var model = product.Adapt<ProductModel>();

                await _pipeline.ExecuteAsync(async ct =>
                {
                    await _cache.SetData(cacheKey, model, token: ct);
                }, token);

                return new Result<ProductModel>(model, 200);
            }
            else
            {
                _logger.ResourceNotFound(typeof(Product).Name, id);

                return new Result<ProductModel>(CustomError.ResourceNotFound<Product>(), 404);
            }
        }
    }

    public Result<PagedResult<ProductModel>> GetProducts(PaginationParams paginationParams,
        ProductFilter filter, CancellationToken token)
    {
        var result = _unitOfWork.Products.GetPaged(paginationParams, filter);

        if (result.Items.Count != 0)
        {
            foreach (var item in result.Items)
            {
                _logger.ResourceReturned(typeof(Product).Name, item.Id);
            }

            return new Result<PagedResult<ProductModel>>(result.Adapt<PagedResult<ProductModel>>(), 200);
        }
        else
        {
            _logger.FilteredResourcesNotFound(typeof(Product).Name);

            return new Result<PagedResult<ProductModel>>(CustomError
                .ResourceNotFound<Product>(), 404);
        }
    }

    public async Task<Result> UpdateProductAsync(Guid id, UpdateProductModel productModel, CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, token);

        if (product is null)
        {
            _logger.ResourceToUpdateNotFound(typeof(Product).Name);

            return Result.Failure(CustomError.ResourceNotFound<Product>(), 404);
        }

        var lockKey = $"lock:product:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            productModel.Adapt(product);

            foreach (var item in productModel.Images)
            {
                if (item.Action is UpdateImageAction.Delete)
                {
                    var uri = new Uri(item.Url!);
                    var cleanPath = uri.AbsolutePath.TrimStart('/');
                    var key = cleanPath.Substring(cleanPath.IndexOf('/') + 1);

                    await _unitOfWork.Files.DeleteFileAsync(key, token);

                    var imageEntity = product.Images.FirstOrDefault(img => img.Url == item.Url);
                    if (imageEntity is not null)
                        await _unitOfWork.Images.Delete(imageEntity);
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
                    var url = await _unitOfWork.Files.UploadFileAsync(key, fileStream, token);
                    image.Url = url;

                    await _unitOfWork.Images.AddAsync(image, token);
                }
            }

            await _unitOfWork.SaveChangesAsync(token);

            await _pipeline.ExecuteAsync(async ct =>
            {
                var cacheKey = $"product:{id}";
                await _cache.RemoveData(cacheKey, ct);
            }, token);

            _logger.ResourceUpdated(typeof(Product).Name, product.Id);

            return Result.Success(204);
        }

    }
}
