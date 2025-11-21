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

public class ProviderService(IUnitOfWork unitOfWork, ILogger<ProviderService> logger) : IProviderService
{
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await unitOfWork.Providers.AddAsync(provider, token);

        await unitOfWork.SaveChangesAsync(token);

        logger.ResourceAdded(typeof(Provider).Name, provider.Id);

        return Result.Success(201);
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.ResourceDeleted(typeof(Provider).Name, provider.Id);

            return Result.Success(204);
        }
        else
        {
            logger.ResourceToDeleteNotFound(typeof(Provider).Name);

            return Result
                .Failure(CustomError.ResourceNotFound<Provider>(), 204);
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            logger.ResourceReturned(typeof(Provider).Name, provider.Id);

            return new Result<ProviderModel>(provider.Adapt<ProviderModel>(), 200);
        }
        else
        {
            logger.ResourceNotFound(typeof(Provider).Name, id);

            return new Result<ProviderModel>(CustomError.ResourceNotFound<Provider>(), 404);
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
                logger.ResourceReturned(typeof(Provider).Name, item.Id);
            }

            return new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>(), 200);
        }
        else
        {
            logger.FilteredResourcesNotFound(typeof(Provider).Name);

            return new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound<Provider>(), 404);
        }    
    }

    public async Task<Result> UpdateProviderAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            logger.ResourceToUpdateNotFound(typeof(Provider).Name);

            return Result.Failure(CustomError.ResourceNotFound<Provider>(), 404);
        }
        else
        {
            providerModel.Adapt(provider);

            await unitOfWork.SaveChangesAsync(token);

            logger.ResourceUpdated(typeof(Provider).Name, provider.Id);

            return Result.Success(204);
        }
    }
}
