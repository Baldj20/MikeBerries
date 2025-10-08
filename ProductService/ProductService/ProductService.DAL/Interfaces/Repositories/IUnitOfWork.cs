namespace ProductService.DAL.Interfaces.Repositories;

public interface IUnitOfWork
{
    IProviderRepository Providers { get; }
    IProductImageRepository Images { get; }
    Task SaveChangesAsync();
}
