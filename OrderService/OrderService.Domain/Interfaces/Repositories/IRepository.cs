namespace OrderService.Domain.Interfaces.Repositories;

public interface IRepository<T>
{
    Task AddAsync(T entity, CancellationToken token);
    void Delete(T entity);
    void Update(T entity);
}
