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

public class ProviderService(IUnitOfWork unitOfWork, ILogger<ProviderService> logger) : IProviderService
{
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await unitOfWork.Providers.AddAsync(provider, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.LogInformation(LoggingConstants.RESOURCE_ADDED, 
            typeof(Provider).Name, 
            provider.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants.RESOURCE_DELETED, 
                typeof(Provider).Name, 
                id);

            return Result.Success();
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCE_TO_DELETE_NOT_FOUND, 
                typeof(Provider).Name);

            var errorMessage = LoggingConstants.RESOURCE_TO_DELETE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

            return Result
                .Failure(CustomError.ResourceNotFound(errorMessage));
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            logger.LogInformation(LoggingConstants.RESOURCE_RETURNED, 
                typeof(Provider).Name,
                id);

            return new Result<ProviderModel>(provider.Adapt<ProviderModel>());
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCE_NOT_FOUND, 
                typeof(Provider).Name,
                id);

            var errorMessage = LoggingConstants.RESOURCE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

            errorMessage = errorMessage
                .Replace("{ResourceId}", id.ToString());

            return new Result<ProviderModel>(CustomError.ResourceNotFound(errorMessage));
        }
    }

    public Result<List<ProviderModel>> GetProviders(PaginationParams paginationParams, 
        ProviderFilter filter, CancellationToken token)
    {
        var result = unitOfWork.Providers.GetPaged(paginationParams, filter);

        if (result.Count != 0)
        {
            foreach (var item in result)
            {
                logger.LogInformation(LoggingConstants.RESOURCE_RETURNED, 
                    typeof(Provider).Name,
                    item.Id);
            }

            return new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>());
        }
        else
        {
            logger.LogWarning(LoggingConstants.RESOURCES_FILTERED_NOT_FOUND,
                typeof(Provider).Name);

            var errorMessage = LoggingConstants.RESOURCES_FILTERED_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

            return new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound(errorMessage));
        }    
    }

    public async Task<Result> UpdateProviderAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            logger.LogWarning(LoggingConstants.RESOURCE_TO_UPDATE_NOT_FOUND, 
                typeof(Provider).Name);

            var errorMessage = LoggingConstants.RESOURCE_TO_UPDATE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

            return Result.Failure(CustomError.ResourceNotFound(errorMessage));
        }
        else
        {
            providerModel.Adapt(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants.RESOURCE_UPDATED,
                typeof(Provider).Name,
                id);

            return Result.Success();
        }
    }
}
