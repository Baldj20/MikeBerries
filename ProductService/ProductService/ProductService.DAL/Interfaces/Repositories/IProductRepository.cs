using ProductService.DAL.Entities;
using ProductService.DAL.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    public IQueryable<Product> GetPagedAsync(PaginationParams paginationParams, 
        ProductFilter filter, CancellationToken token);
}
