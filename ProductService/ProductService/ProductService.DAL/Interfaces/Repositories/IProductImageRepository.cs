using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProductImageRepository : IRepository<ProductImage>
{
    public Task<ICollection<ProductImage>> GetAllProductImagesByArticle(string article);
}
