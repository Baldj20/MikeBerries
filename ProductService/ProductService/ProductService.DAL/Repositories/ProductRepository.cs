using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

internal class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(MikeBerriesDBContext context)
        : base(context)
    {
        
    }

    public async Task<Product?> GetByArticle(string article)
    {
        var product = await context.Products.Where(p => p.Article == article)
            .FirstOrDefaultAsync();

        return product;
    }
}
