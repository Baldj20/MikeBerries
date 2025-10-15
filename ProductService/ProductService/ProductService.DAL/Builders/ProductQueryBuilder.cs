using ProductService.DAL.Entities;

namespace ProductService.DAL.Builders;

public static class ProductQueryBuilder
{
    public static IQueryable<Product> WithTitle(this IQueryable<Product> query, string? title)
    {
        if (title is not null)
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        return query;
    }
    public static IQueryable<Product> WithProvider(this IQueryable<Product> query, string? provider)
    {
        if (provider is not null)
        {
            query = query.Where(p => p.Provider.Name.Contains(provider));
        }

        return query;
    }
    public static IQueryable<Product> HasMaxPrice(this IQueryable<Product> query, decimal? maxPrice)
    {
        if (maxPrice is not null)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        return query;
    }
    public static IQueryable<Product> HasMinPrice(this IQueryable<Product> query, decimal? minPrice)
    {
        if (minPrice is not null)
        {
            query = query.Where(p => p.Price >= minPrice);
        }

        return query;
    }
}
