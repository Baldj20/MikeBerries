using ProductService.DAL.Interfaces.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IPagedRepository<T>
{
    PagedResult<T> GetPaged(PaginationParams paginationParams, IFilter<T> filter);
}
