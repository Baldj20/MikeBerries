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
    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _context.Set<T>().Where(e => e.Id == id)
            .FirstOrDefaultAsync();

        return entity;
    }

    public Task Update(T entity)
    {
        _context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }
}
