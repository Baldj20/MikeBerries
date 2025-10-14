namespace ProductService.DAL.Interfaces.Repositories;

public interface IRepository<T> : IPagedRepository<T>
{
    Task AddAsync(T entity, CancellationToken token);
    Task Delete(T entity);
    Task Update(T entity);
    Task<T?> GetByIdAsync(Guid id, CancellationToken token);
}
