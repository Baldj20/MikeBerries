using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.BLL.Interfaces.Services;

public interface IProviderService
{
    Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token);
    Task<Result> DeleteProviderAsync(Guid id, CancellationToken token);
    Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token);
    Result<List<ProviderModel>> GetProviders(PaginationParams paginationParams,
        ProviderFilter filter, CancellationToken token);
    Task<Result> UpdateProviderAsync(Guid id, ProviderModel providerModel, CancellationToken token);
}
