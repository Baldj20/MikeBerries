using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly MikeBerriesDBContext _context;
    public Repository(MikeBerriesDBContext context)
    {
        _context = context;
    }
    public async Task Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
