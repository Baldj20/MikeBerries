using ProductService.DAL.Interfaces.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IPagedRepository<T>
{
    IQueryable<T> GetPaged(PaginationParams paginationParams, IFilter<T> filter);
}
