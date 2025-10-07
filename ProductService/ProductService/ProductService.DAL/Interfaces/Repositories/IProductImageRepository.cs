using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

internal interface IProductImageRepository : IRepository<ProductImage>
{
    public Task<ICollection<ProductImage>> GetAllProductImagesByArticle(string article);
}