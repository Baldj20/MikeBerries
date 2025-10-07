using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ProductService.DAL.Repositories;

internal class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
{
    public ProductImageRepository(MikeBerriesDBContext context)
        : base(context)
    {
        
    }
    public async Task<ICollection<ProductImage>> GetAllProductImagesByArticle(string article)
    {
        var images = await context.ProductImages.Where(i => i.ProductArticle == article).ToListAsync();
       
        return images;
    }
}