using ProductService.DAL.Entities;

namespace ProductService.DAL.Builders;

public static class ProductQueryBuilder
{
    public static IQueryable<Product> WithTitle(this IQueryable<Product> query, string? title)
    {
        if (title is not null)
        {
            query.Where(p => p.Title == title);
        }

        return query;
    }
    public static IQueryable<Product> WithProvider(this IQueryable<Product> query, string? provider)
    {
        if (provider is not null)
        {
            query.Where(p => p.Provider.Name == provider);
        }

        return query;
    }
    public static IQueryable<Product> HasMaxPrice(this IQueryable<Product> query, decimal? maxPrice)
    {
        if (maxPrice is not null)
        {
            query.Where(p => p.Price <= maxPrice);
        }

        return query;
    }
    public static IQueryable<Product> HasMinPrice(this IQueryable<Product> query, decimal? minPrice)
    {
        if (minPrice is not null)
        {
            query.Where(p => p.Price >= minPrice);
        }

        return query;
    }
    public static IQueryable<Product> TakePage(this IQueryable<Product> query, PaginationParams? paginationParams)
    {
        if (paginationParams is not null)
        {
            var (page, pageSize) = paginationParams;

            query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        return query;
    }
}
