using ProductService.DAL.Entities;
using ProductService.DAL.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    public IQueryable<Product> GetPaged(PaginationParams paginationParams, 
        ProductFilter filter);
}
