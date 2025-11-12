using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProviderRepository(MikeBerriesDBContext context) : Repository<Provider>(context), IProviderRepository
{
    public async Task<Provider?> GetByEmailAsync(string email, CancellationToken token)
    {
        var provider = await Context.Providers.Where(p => p.Email == email)
            .FirstOrDefaultAsync();

        return provider;
    }
}
