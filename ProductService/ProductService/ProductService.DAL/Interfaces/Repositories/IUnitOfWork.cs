namespace ProductService.DAL.Interfaces.Repositories;

public interface IUnitOfWork
{
    Lazy<IProviderRepository> Providers { get; }
    Lazy<IProductImageRepository> Images { get; }
    Task SaveChangesAsync(CancellationToken token);
}
