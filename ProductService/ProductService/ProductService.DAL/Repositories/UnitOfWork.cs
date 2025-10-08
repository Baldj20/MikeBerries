using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MikeBerriesDBContext _context;
    public UnitOfWork(MikeBerriesDBContext context)
    {
        _context = context;

        Providers = new Lazy<IProviderRepository>(() => new ProviderRepository(_context));
        Images = new Lazy<IProductImageRepository>(() => new ProductImageRepository(_context));
    }

    public Lazy<IProviderRepository> Providers { get; } 
    public Lazy<IProductImageRepository> Images { get; }
    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }
}
