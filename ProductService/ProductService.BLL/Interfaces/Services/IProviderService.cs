using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;
using System.Security.Claims;

namespace ProductService.BLL.Interfaces.Services;

public interface IProviderService
{
    Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token);
    Task<Result> DeleteProviderAsync(Guid id, CancellationToken token);
    Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token);
    Result<PagedResult<ProviderModel>> GetProviders(PaginationParams paginationParams,
        ProviderFilter filter, CancellationToken token);
    Task<Result> UpdateProviderAsync(Guid id, ProviderModel providerModel, CancellationToken token);
}
