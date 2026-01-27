namespace OrderService.Domain.Interfaces.Repositories;

public interface IRepository<T>
{
    Task AddAsync(T entity, CancellationToken token);
    Task DeleteAsync(T entity, CancellationToken token);
    Task UpdateAsync(T entity, CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}
