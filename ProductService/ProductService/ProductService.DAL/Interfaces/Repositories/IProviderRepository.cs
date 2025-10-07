using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

internal interface IProviderRepository : IRepository<Provider>
{
    public Task<Provider?> GetByEmail(string email);
}
