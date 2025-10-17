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

public class ProviderService(IUnitOfWork unitOfWork, ILogger logger) : IProviderService
{
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await unitOfWork.Providers.AddAsync(provider, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.Information(LoggingConstants<Provider>.RESOURCE_ADDED, provider.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.Information(LoggingConstants<Provider>.RESOURCE_DELETED, id);

            return Result.Success();
        }
        else
        {
            logger.Warning(LoggingConstants<Provider>.RESOURCE_TO_DELETE_NOT_FOUND);

            return Result
                .Failure(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_TO_DELETE_NOT_FOUND));
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            logger.Information(LoggingConstants<Provider>.RESOURCE_RETURNED, id);

            return new Result<ProviderModel>(provider.Adapt<ProviderModel>());
        }
        else
        {
            logger.Warning(LoggingConstants<Provider>.RESOURCE_NOT_FOUND);

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
                logger.Information(LoggingConstants<Provider>.RESOURCE_RETURNED, item.Id);
            }

            return new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>());
        }
        else
        {
            logger.Warning(LoggingConstants<Provider>.RESOURCES_FILTERED_NOT_FOUND);

            return new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound(LoggingConstants<Provider>.RESOURCES_FILTERED_NOT_FOUND));
        }    
    }

    public async Task<Result> UpdateAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            logger.Warning(LoggingConstants<Provider>.RESOURCE_TO_UPDATE_NOT_FOUND);

            return Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_TO_UPDATE_NOT_FOUND));
        }
        else
        {
            providerModel.Adapt(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.Information(LoggingConstants<Provider>.RESOURCE_UPDATED, id);

            return Result.Success();
        }
    }
}
