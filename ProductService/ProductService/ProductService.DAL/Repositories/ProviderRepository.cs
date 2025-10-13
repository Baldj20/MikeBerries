using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Builders;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProviderRepository(MikeBerriesDBContext context) : Repository<Provider>(context), IProviderRepository
{
    public async Task<Provider?> GetByEmailAsync(string email, CancellationToken token)
    {
        var provider = await context.Providers.Where(p => p.Email == email)
            .FirstOrDefaultAsync();

        return provider;
    }

    public IQueryable<Provider> GetPaged(PaginationParams paginationParams, ProviderFilter filter)
    {
        var(name, product) = filter;

        var initialQuery = Context.Providers.AsQueryable();

        var query = initialQuery
                        .WithName(name)
                        .WithProduct(product)
                        .TakePage(paginationParams);

        return query;
    }
}
