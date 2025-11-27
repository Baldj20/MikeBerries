using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MikeBerriesDBContext _context;
    private readonly Lazy<IProviderRepository> _providers;
    private readonly Lazy<IProductImageRepository> _images;
    private readonly Lazy<IProductRepository> _products;
    private readonly Lazy<IFileRepository> _files;

    public UnitOfWork(MikeBerriesDBContext context, MinioStorage minioStorage)
    {
        _context = context;

        _providers = new Lazy<IProviderRepository>(() => new ProviderRepository(_context));
        _images = new Lazy<IProductImageRepository>(() => new ProductImageRepository(_context));
        _products = new Lazy<IProductRepository>(() => new ProductRepository(_context));
        _files = new Lazy<IFileRepository>(() => new MinioFileRepository(minioStorage));
    }

    public IProviderRepository Providers => _providers.Value;
    public IProductImageRepository Images => _images.Value;
    public IProductRepository Products => _products.Value;
    public IFileRepository Files => _files.Value;
    public async Task SaveChangesAsync(CancellationToken token)
    {
        await _context.SaveChangesAsync(token);
    }
}
