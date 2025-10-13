using ProductService.DAL.Builders;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    public IQueryable<Product> GetPaged(PaginationParams paginationParams, ProductFilter filter)
    {
        var (title, provider, minPrice, maxPrice) = filter;

        var initialQuery = Context.Products.AsQueryable();

        var query = initialQuery
                        .WithTitle(title)
                        .WithProvider(provider)
                        .HasMinPrice(minPrice)
                        .HasMaxPrice(maxPrice)
                        .TakePage(paginationParams);
        
        return query;
    }
}
