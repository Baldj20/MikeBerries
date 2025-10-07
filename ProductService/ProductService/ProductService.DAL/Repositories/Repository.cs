using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
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

    public async Task<T?> GetById(Guid id)
    {
        var entity = await _context.Set<T>().Where(e => e.Id == id)
            .FirstOrDefaultAsync();

        return entity;
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
