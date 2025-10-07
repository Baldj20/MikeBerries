using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DAL.Repositories;

internal class ProductImageRepository(MikeBerriesDBContext context) : Repository<ProductImage>(context), IProductImageRepository
{
    public async Task<ICollection<ProductImage>> GetAllProductImagesByArticle(string article, CancellationToken token)
    {
        var images = await context.ProductImages.Where(i => i.ProductArticle == article).ToListAsync(token);
       
        return images;
    }
}
