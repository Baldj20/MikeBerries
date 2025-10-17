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

public class ProviderService(IUnitOfWork unitOfWork) : IProviderService
{
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await unitOfWork.Providers.AddAsync(provider, token);

        await unitOfWork.SaveChangesAsync(token);

        Log.Information(LoggingConstants<Provider>.RESOURCE_ADDED, provider.Id);

        return Result.Success();
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            await unitOfWork.SaveChangesAsync(token);

            Log.Information(LoggingConstants<Provider>.RESOURCE_DELETED, id);

            return Result.Success();
        }
        else
        {
            Log.Warning(LoggingConstants<Provider>.RESOURCE_NOT_FOUND, ActionConstants.ACTION_DELETE, id);

            return Result
                .Failure(CustomError.ResourceNotFound("resource to delete is not found"));
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            Log.Information(LoggingConstants<Provider>.RESOURCE_RETURNED, id);

            return new Result<ProviderModel>(provider.Adapt<ProviderModel>());
        }
        else
        {
            Log.Warning(LoggingConstants<Provider>.RESOURCE_NOT_FOUND,
                ActionConstants.ACTION_GET, id);

            return new Result<ProviderModel>(CustomError.ResourceNotFound("resource with this id does not exist"));
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
                Log.Information(LoggingConstants<Provider>.RESOURCE_RETURNED, item.Id);
            }

            return new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>());
        }
        else
        {
            Log.Warning(LoggingConstants<Provider>.RESOURCES_FILTERED_NOT_FOUND);

            return new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound("resources with these filters do not exist"));
        }    
    }

    public async Task<Result> UpdateAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
        {
            Log.Warning(LoggingConstants<Provider>.RESOURCE_NOT_FOUND, ActionConstants.ACTION_UPDATE, id);

            return Result.Failure(CustomError.ResourceNotFound("resource to update does not exist"));
        }
        else
        {
            providerModel.Adapt(provider);

            await unitOfWork.SaveChangesAsync(token);

            Log.Information(LoggingConstants<Provider>.RESOURCE_UPDATED, id);

            return Result.Success();
        }
    }
}
