using ProductService.DAL.Interfaces.Filters;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IPagedRepository<T>
{
    List<T> GetPaged(PaginationParams paginationParams, IFilter<T> filter);
}
