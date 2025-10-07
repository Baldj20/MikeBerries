using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<Product?> GetByArticle(string article)
    {
        var product = await context.Products.Where(p => p.Article == article)
            .FirstOrDefaultAsync();

        return product;
    }
}
