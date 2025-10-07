using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

internal interface IProductRepository : IRepository<Product>
{
    public Task<Product?> GetByArticle(string article);
}