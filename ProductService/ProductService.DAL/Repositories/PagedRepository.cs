using ProductService.DAL.Interfaces.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class PagedRepository<T>(MikeBerriesDBContext сontext) : IPagedRepository<T> where T : class
{
    protected MikeBerriesDBContext Context { get; } = сontext;
    public List<T> GetPaged(PaginationParams paginationParams,
        IFilter<T> filter)
    {
        var initialQuery = Context.Set<T>().AsQueryable();

        var query = filter.Apply(initialQuery);

        query = query.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                     .Take(paginationParams.PageSize);

        return query.ToList();
    }
}
