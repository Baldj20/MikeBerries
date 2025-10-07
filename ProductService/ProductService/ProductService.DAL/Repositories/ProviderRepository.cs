using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

internal class ProviderRepository : Repository<Provider>, IProviderRepository
{
    public ProviderRepository(MikeBerriesDBContext context)
        : base(context)
    {
        
    }
    public async Task<Provider?> GetByEmail(string email)
    {
        var provider = await context.Providers.Where(p => p.Email == email)
            .FirstOrDefaultAsync();

        return provider;
    }
}