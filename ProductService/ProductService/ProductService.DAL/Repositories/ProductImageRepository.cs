using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DAL.Repositories;

public class ProductImageRepository(MikeBerriesDBContext context) : Repository<ProductImage>(context), IProductImageRepository
{
    public async Task<ICollection<ProductImage>> GetAllImagesByProductIdAsync(Guid id, CancellationToken token)
    {
        var images = await context.ProductImages
            .Where(i => i.Id == id)
            .AsNoTracking()
            .ToListAsync(token);
       
        return images;
    }
}
