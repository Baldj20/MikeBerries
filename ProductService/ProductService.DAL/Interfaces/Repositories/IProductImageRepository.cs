using ProductService.DAL.Entities;

namespace ProductService.DAL.Interfaces.Repositories;

public interface IProductImageRepository : IRepository<ProductImage>
{
    Task<ICollection<ProductImage>> GetAllImagesByProductIdAsync(Guid id, CancellationToken token);
}
