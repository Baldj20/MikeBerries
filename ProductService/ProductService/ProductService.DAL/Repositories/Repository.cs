using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

internal class Repository<T> : IRepository<T> where T : class
{
    protected readonly MikeBerriesDBContext context;
    public Repository(MikeBerriesDBContext context)
    {
        this.context = context;
    }
    public async Task Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public async Task Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task Update(T entity)
    {
        context.Set<T>().Update(entity);
    }
}