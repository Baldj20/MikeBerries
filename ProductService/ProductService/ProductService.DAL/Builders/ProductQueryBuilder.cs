using ProductService.DAL.Entities;

namespace ProductService.DAL.Builders;

public class ProductQueryBuilder
{
    private IQueryable<Product> query;
    public ProductQueryBuilder(IQueryable<Product> query)
    {
        this.query = query;
    }
    public ProductQueryBuilder WithTitle(string? title)
    {
        if (title is not null)
        {
            query.Where(p => p.Title == title);
        }

        return this;
    }
    public ProductQueryBuilder WithProvider(string? provider)
    {
        if (provider is not null)
        {
            query.Where(p => p.Provider.Name == provider);
        }

        return this;
    }
    public ProductQueryBuilder HasMaxPrice(decimal? maxPrice)
    {
        if (maxPrice is not null)
        {
            query.Where(p => p.Price <= maxPrice);
        }

        return this;
    }
    public ProductQueryBuilder HasMinPrice(decimal? minPrice)
    {
        if (minPrice is not null)
        {
            query.Where(p => p.Price >= minPrice);
        }

        return this;
    }
    public ProductQueryBuilder TakePage(PaginationParams? paginationParams)
    {
        if (paginationParams is not null)
        {
            var (page, pageSize) = paginationParams;

            query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        return this;
    }
    public IQueryable<Product> Build()
    {
        return query;
    }
}
