using OrderService.Domain.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

public class Repository<T>(OrderServiceDbContext context) : IRepository<T> where T : class
{
    protected OrderServiceDbContext Context => context;
    
    public async Task AddAsync(T entity, CancellationToken token)
    {
        await Context.Set<T>().AddAsync(entity, token);
    }

    public Task DeleteAsync(T entity, CancellationToken token)
    {
        Context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(T entity, CancellationToken token)
    {
        Context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }
}
