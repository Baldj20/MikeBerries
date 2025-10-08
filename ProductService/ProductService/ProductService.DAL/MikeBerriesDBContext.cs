using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;

namespace ProductService.DAL;

public class MikeBerriesDBContext(DbContextOptions<MikeBerriesDBContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Provider> Providers { get; set; }
}
