using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProviderRepository(MikeBerriesDBContext context) : Repository<Provider>(context), IProviderRepository
{
    public async Task<Provider?> GetByEmailAsync(string email, CancellationToken token)
    {
        var provider = await Context.Providers.Where(p => p.Email == email)
            .FirstOrDefaultAsync(token);

        return provider;
    }

    public List<Provider> GetPaged(PaginationParams paginationParams, IFilter<Provider> filter)
    {
        var initialQuery = Context.Providers.AsQueryable();

        var query = filter.Apply(initialQuery);

        query = query.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                     .Take(paginationParams.PageSize);

        return query.ToList();
    }
}
