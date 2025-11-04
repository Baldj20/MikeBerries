using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IUnitOfWork
{
    IProviderRepository Providers { get; }
    IProductImageRepository Images { get; }
    IProductRepository Products { get; }
    Task SaveChangesAsync(CancellationToken token);
}
