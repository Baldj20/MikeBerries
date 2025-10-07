namespace ProductService.DAL.Interfaces.Repositories;

internal interface IRepository<T>
{
    public Task Add(T entity);
    public Task Delete(T entity);
    public Task Update(T entity);
}
