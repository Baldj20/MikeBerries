using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class Repository<T>(MikeBerriesDBContext context) : PagedRepository<T>(context), IRepository<T> where T : BaseEntity
{
    protected MikeBerriesDBContext Context => context;
    public async Task AddAsync(T entity, CancellationToken token)
    {
        await Context.Set<T>().AddAsync(entity, token);
    }

    public Task Delete(T entity)
    {
        Context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await Context.Set<T>().Where(e => e.Id == id)
            .FirstOrDefaultAsync();

        return entity;
    }

    public Task Update(T entity)
    {
        Context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }
}
