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

public class ProviderService : IProviderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheRepository _cache;
    private readonly ResiliencePipeline _pipeline;
    private readonly ILogger<ProviderService> _logger;
    private readonly IDistributedLockProvider _lockProvider;

    public ProviderService(IUnitOfWork unitOfWork, ICacheRepository cache,
        ILogger<ProviderService> logger,
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
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await _unitOfWork.Providers.AddAsync(provider, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.ResourceAdded(typeof(Provider).Name, provider.Id);

        return Result.Success(201);
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await _unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            _logger.ResourceToDeleteNotFound(typeof(Provider).Name);

            return Result
                .Failure(CustomError.ResourceNotFound<Provider>(), 204);
        }

        var lockKey = $"lock:provider:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            await _unitOfWork.Providers.Delete(provider);

            await _unitOfWork.SaveChangesAsync(token);

            await _pipeline.ExecuteAsync(async ct =>
            {
                var cacheKey = $"provider:{id}";
                await _cache.RemoveData(cacheKey, ct);
            }, token);

            _logger.ResourceDeleted(typeof(Provider).Name, provider.Id);

            return Result.Success(204);
        }
    }
    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var cacheKey = $"provider:{id}";

        var providerModel = await _pipeline.ExecuteAsync(async ct =>
        {
            return await _cache.GetData<ProviderModel>(cacheKey, ct);
        }, token);

        if (providerModel is not null)
        {
            return new Result<ProviderModel>(providerModel, 200);
        }

        var lockKey = $"lock:provider:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            providerModel = await _pipeline.ExecuteAsync(async ct =>
            {
                return await _cache.GetData<ProviderModel>(cacheKey, ct);
            }, token);

            if (providerModel is not null)
            {
                return new Result<ProviderModel>(providerModel, 200);
            }

            var provider = await _unitOfWork.Providers.GetByIdAsync(id, token);

            if (provider is not null)
            {
                _logger.ResourceReturned(typeof(Provider).Name, provider.Id);

                var model = provider.Adapt<ProviderModel>();

                await _pipeline.ExecuteAsync(async ct =>
                {
                    await _cache.SetData(cacheKey, model, token: ct);
                }, token);

                return new Result<ProviderModel>(model, 200);
            }
            else
            {
                _logger.ResourceNotFound(typeof(Provider).Name, id);

                return new Result<ProviderModel>(CustomError.ResourceNotFound<Provider>(), 404);
            }
        }
    }

    public Result<PagedResult<ProviderModel>> GetProviders(PaginationParams paginationParams,
        ProviderFilter filter, CancellationToken token)
    {
        var result = _unitOfWork.Providers.GetPaged(paginationParams, filter);

        if (result.Items.Count != 0)
        {
            foreach (var item in result.Items)
            {
                _logger.ResourceReturned(typeof(Provider).Name, item.Id);
            }

            return new Result<PagedResult<ProviderModel>>(result.Adapt<PagedResult<ProviderModel>>(), 200);
        }
        else
        {
            _logger.FilteredResourcesNotFound(typeof(Provider).Name);

            return new Result<PagedResult<ProviderModel>>(CustomError
                .ResourceNotFound<Provider>(), 404);
        }
    }

    public async Task<Result> UpdateProviderAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await _unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            _logger.ResourceToUpdateNotFound(typeof(Provider).Name);

            return Result.Failure(CustomError.ResourceNotFound<Provider>(), 404);
        }

        var lockKey = $"lock:provider:{id}";

        await using (await _lockProvider.AcquireLockAsync(lockKey, cancellationToken: token))
        {
            providerModel.Adapt(provider);

            await _unitOfWork.SaveChangesAsync(token);

            await _pipeline.ExecuteAsync(async ct =>
            {
                var cacheKey = $"provider:{id}";
                await _cache.RemoveData(cacheKey, ct);
            }, token);

            _logger.ResourceUpdated(typeof(Provider).Name, provider.Id);

            return Result.Success(204);
        }
    }
}
