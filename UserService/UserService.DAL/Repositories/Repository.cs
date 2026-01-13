using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class Repository<T>(UserServiceDbContext dbContext) : IRepository<T> where T : class
{
    protected UserServiceDbContext Context => dbContext;
    public async Task AddAsync(T entity, CancellationToken token)
    {   
        await Context.Set<T>().AddAsync(entity, token);
    }

    public Task Delete(T entity)
    {
        Context.Set<T>().Remove(entity);

        return Task.CompletedTask;
    }

    public Task Update(T entity)
    {
        Context.Set<T>().Update(entity);

        return Task.CompletedTask;
    }
}
