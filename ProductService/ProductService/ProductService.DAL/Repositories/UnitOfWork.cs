using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MikeBerriesDBContext _context;
    private Lazy<IProviderRepository> _providers;
    private Lazy<IProductImageRepository> _images;
    private Lazy<IRepository<Product>> _products;
    public UnitOfWork(MikeBerriesDBContext context)
    {
        _context = context;

        _providers = new Lazy<IProviderRepository>(() => new ProviderRepository(_context));
        _images = new Lazy<IProductImageRepository>(() => new ProductImageRepository(_context));
        _products = new Lazy<IRepository<Product>>(() => new Repository<Product>(_context));
    }

    public IProviderRepository Providers => _providers.Value;
    public IProductImageRepository Images => _images.Value;
    public IRepository<Product> Products => _products.Value;
    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }
}
