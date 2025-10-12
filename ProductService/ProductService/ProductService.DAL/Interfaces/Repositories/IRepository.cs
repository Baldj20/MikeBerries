namespace ProductService.DAL.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task AddAsync(T entity, CancellationToken token);
    public Task Delete(T entity);
    public Task Update(T entity);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken token);
}
