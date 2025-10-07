using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class ProductRepository(MikeBerriesDBContext context) : Repository<Product>(context), IProductRepository
{
    
}
