using ProductService.DAL.Builders;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    public IQueryable<Product> GetPagedAsync(PaginationParams paginationParams, ProductFilter filter, CancellationToken token)
    {
        var (title, provider, minPrice, maxPrice) = filter;

        var initialQuery = _context.Products.AsQueryable();

        var queryBuilder = new ProductQueryBuilder(initialQuery);

        var query = queryBuilder
                        .WithTitle(title)
                        .WithProvider(provider)
                        .HasMinPrice(minPrice)
                        .HasMaxPrice(maxPrice)
                        .TakePage(paginationParams)
                        .Build();
        
        return query;
    }
}
