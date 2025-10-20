using Mapster;
using Microsoft.EntityFrameworkCore;
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

        logger.LogInformation(LoggingConstants<Provider>.RESOURCE_ADDED, provider.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants<Provider>.RESOURCE_DELETED, id);

            return Result.Success();
        }
        else
        {
            logger.LogWarning(LoggingConstants<Provider>.RESOURCE_TO_DELETE_NOT_FOUND);

            return Result
                .Failure(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_TO_DELETE_NOT_FOUND));
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            logger.LogInformation(LoggingConstants<Provider>.RESOURCE_RETURNED, id);

            return new Result<ProviderModel>(provider.Adapt<ProviderModel>());
        }
        else
        {
            logger.LogWarning(LoggingConstants<Provider>.RESOURCE_NOT_FOUND);

            return new Result<ProviderModel>(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_NOT_FOUND));
        }
    }

    public async Task<Result<List<ProviderModel>>> GetProvidersAsync(PaginationParams paginationParams, 
        ProviderFilter filter, CancellationToken token)
    {
        var query = unitOfWork.Providers.GetPaged(paginationParams, filter);

        var result = await query.ToListAsync(token);

        if (result.Count != 0)
        {
            foreach (var item in result)
            {
                logger.LogInformation(LoggingConstants<Provider>.RESOURCE_RETURNED, item.Id);
            }

            return new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>());
        }
        else
        {
            logger.LogWarning(LoggingConstants<Provider>.RESOURCES_FILTERED_NOT_FOUND);

            return new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound(LoggingConstants<Provider>.RESOURCES_FILTERED_NOT_FOUND));
        }    
    }

    public async Task<Result> UpdateAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            logger.LogWarning(LoggingConstants<Provider>.RESOURCE_TO_UPDATE_NOT_FOUND);

            return Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_TO_UPDATE_NOT_FOUND));
        }
        else
        {
            providerModel.Adapt(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.LogInformation(LoggingConstants<Provider>.RESOURCE_UPDATED, id);

            return Result.Success();
        }
    }
}
