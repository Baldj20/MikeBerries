using ProductService.DAL.Interfaces.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class PagedRepository<T>(MikeBerriesDBContext Context) : IPagedRepository<T> where T : class
{
    public IQueryable<T> GetPaged(PaginationParams paginationParams, 
        IFilter<T> filter)
    {
        var initialQuery = Context.Set<T>().AsQueryable();

        var query = filter.Apply(initialQuery);

        return query;
    }
}
