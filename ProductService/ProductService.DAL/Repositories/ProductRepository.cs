using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    public new async Task<Product?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var product = await Context.Products.Where(p => p.Id == id)
            .Include(p => p.Provider)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(token);

        return product;
    }

    public PagedResult<Product> GetPaged(PaginationParams paginationParams, IFilter<Product> filter)
    {
        var initialQuery = Context.Products
            .Include(p => p.Provider)
            .Include(p => p.Images)
            .AsQueryable();

        var query = filter.Apply(initialQuery);

        query = query.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                     .Take(paginationParams.PageSize);

        var items = query.ToList();

        return new PagedResult<Product>
        {
            Items = items,
            CurrentPage = paginationParams.Page,
            TotalPages = query.Count()
        };
    }
}
