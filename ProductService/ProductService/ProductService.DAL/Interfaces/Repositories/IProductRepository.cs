using ProductService.DAL.Entities;
using ProductService.DAL.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    new Task<Product?> GetByIdAsync(Guid id, CancellationToken token);
}
