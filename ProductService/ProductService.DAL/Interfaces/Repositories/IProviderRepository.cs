using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProviderRepository : IRepository<Provider>
{
    Task<Provider?> GetByEmailAsync(string email, CancellationToken token);
}
