using ProductService.DAL.Entities;
using ProductService.DAL.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProviderRepository : IRepository<Provider>
{
    public Task<Provider?> GetByEmailAsync(string email, CancellationToken token);
    public IQueryable<Provider> GetPaged(PaginationParams paginationParams,
        ProviderFilter filter);
}
