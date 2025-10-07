namespace ProductService.DAL.Interfaces.Repositories;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IProviderRepository Providers { get; }
    IProductImageRepository Images { get; }
    public Task SaveChangesAsync();
}
