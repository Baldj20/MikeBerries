using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    public new async Task<Product?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var product = await Context.Products.Where(p => p.Id == id)
            .Include(p => p.Provider)
            .Include(p => p.Images)
            .FirstOrDefaultAsync();

        return product;
    }
}
