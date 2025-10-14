using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductService.BLL.Configurations;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.BLL.Services;

public class ProviderService(IUnitOfWork unitOfWork) : IProviderService
{
    public async Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token)
    {
        var provider = providerModel.Adapt<Provider>();

        await unitOfWork.Providers.AddAsync(provider, token);

        await unitOfWork.SaveChangesAsync(token);

        return Result.Success();
    }

    public async Task<Result> DeleteProviderAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is not null)
        {
            await unitOfWork.Providers.Delete(provider);

            return Result.Success();
        }
        else
        {
            return Result
                .Failure(CustomError.ResourceNotFound("resource to delete is not found"));
        }
    }

    public async Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        return provider is null ?
            new Result<ProviderModel>(CustomError.ResourceNotFound("resource with this id does not exist")) :
            new Result<ProviderModel>(provider.Adapt<ProviderModel>());
    }

    public async Task<Result<List<ProviderModel>>> GetProvidersAsync(PaginationParams paginationParams, ProviderFilter filter, CancellationToken token)
    {
        var query = unitOfWork.Providers.GetPaged(paginationParams, filter);

        var result = await query.ToListAsync(token);

        return result.Count == 0 ?
            new Result<List<ProviderModel>>(CustomError.ResourceNotFound("resources with these filters do not exist")) :
            new Result<List<ProviderModel>>(result.Adapt<List<ProviderModel>>());
    }

    public async Task<Result> UpdateAsync(Guid id, ProviderModel providerModel, CancellationToken token)
    {
        var provider = await unitOfWork.Providers.GetByIdAsync(id, token);

        if (provider is null)
            return Result.Failure(CustomError.ResourceNotFound("resource to update does not exist"));

        providerModel.Adapt(provider);

        await unitOfWork.SaveChangesAsync(token);

        return Result.Success();
    }
}
