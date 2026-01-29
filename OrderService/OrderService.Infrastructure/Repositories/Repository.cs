using OrderService.Domain.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

public class Repository<T>(OrderServiceDbContext context) : IRepository<T> where T : class
{
    protected OrderServiceDbContext Context => context;
    
    public async Task AddAsync(T entity, CancellationToken token)
    {
        await Context.Set<T>().AddAsync(entity, token);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }
}
