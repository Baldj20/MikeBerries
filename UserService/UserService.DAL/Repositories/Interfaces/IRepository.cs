namespace UserService.DAL.Repositories.Interfaces;

public interface IRepository<T>
{
    Task AddAsync(T entity, CancellationToken token);
    Task Delete(T entity);
    Task Update(T entity);
    Task SaveChangesAsync(CancellationToken token);
}
