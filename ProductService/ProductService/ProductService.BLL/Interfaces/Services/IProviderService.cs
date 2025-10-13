using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.BLL.Interfaces.Services;

public interface IProviderService
{
    public Task<Result> AddProviderAsync(ProviderModel providerModel, CancellationToken token);
    public Task<Result> DeleteProviderAsync(Guid id, CancellationToken token);
    public Task<Result<ProviderModel>> GetProviderByIdAsync(Guid id, CancellationToken token);
    public Task<Result<List<ProviderModel>>> GetProvidersAsync(PaginationParams paginationParams,
        ProviderFilter filter, CancellationToken token);
    public Task<Result> UpdateAsync(Guid id, ProviderModel providerModel, CancellationToken token);
}
