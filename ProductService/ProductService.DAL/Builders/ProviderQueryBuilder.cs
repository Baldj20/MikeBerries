using ProductService.DAL.Entities;

namespace ProductService.DAL.Builders;

public static class ProviderQueryBuilder
{
    public static IQueryable<Provider> WithName(this IQueryable<Provider> query, string? name)
    {
        if (name is not null)
        {
            query = query.Where(p => p.Name.Contains(name));
        }

        return query;
    }
    public static IQueryable<Provider> WithProduct(this IQueryable<Provider> query, string? product)
    {
        if (product is not null)
        {
            query = query.Where(p => p.Products
                         .Any(prod => prod.Title.Contains(product)));
        }

        return query;
    }
    public static IQueryable<Provider> TakePage(this IQueryable<Provider> query, PaginationParams? paginationParams)
    {
        if (paginationParams is not null)
        {
            var (page, pageSize) = paginationParams;

            query = query
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);
        }

        return query;
    }
}
