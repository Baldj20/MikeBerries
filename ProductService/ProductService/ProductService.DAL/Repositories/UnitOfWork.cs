using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class UnitOfWork(MikeBerriesDBContext context) : IUnitOfWork
{
    public IProductRepository Products { get; } = new ProductRepository(context);

    public IProviderRepository Providers { get; } = new ProviderRepository(context);

    public IProductImageRepository Images { get; } = new ProductImageRepository(context);

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
