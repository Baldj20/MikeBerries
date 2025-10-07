using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly MikeBerriesDBContext _context;
    public IProductRepository Products { get; private set; }

    public IProviderRepository Providers { get; private set; }

    public IProductImageRepository Images { get; private set; }
    public UnitOfWork(MikeBerriesDBContext context)
    {
        _context = context;

        Products = new ProductRepository(context);
        Providers = new ProviderRepository(context);
        Images = new ProductImageRepository(context);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
