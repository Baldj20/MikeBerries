namespace ProductService.DAL.Interfaces.Repositories;

internal interface IUnitOfWork
{
    IProductRepository Products { get; }
    IProviderRepository Providers { get; }
    IProductImageRepository Images { get; }
    public Task Complete();
}