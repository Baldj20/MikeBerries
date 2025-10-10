using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IUnitOfWork
{
    Lazy<IProviderRepository> Providers { get; }
    Lazy<IProductImageRepository> Images { get; }
    Lazy<IRepository<Product>> Products { get; }
    Task SaveChangesAsync(CancellationToken token);
}
