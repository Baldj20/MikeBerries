using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProviderRepository : IRepository<Provider>, IPagedRepository<Provider>
{
    Task<Provider?> GetByEmailAsync(string email, CancellationToken token);
}
