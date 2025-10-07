namespace ProductService.DAL.Interfaces.Repositories;

public interface IRepository<T>
{
    public Task Add(T entity);
    public Task Delete(T entity);
    public Task Update(T entity);
}
