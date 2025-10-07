using Microsoft.EntityFrameworkCore;
using ProductService.DAL.Entities;

namespace ProductService.DAL;

public class MikeBerriesDBContext : DbContext
{
    public MikeBerriesDBContext(DbContextOptions<MikeBerriesDBContext> options) 
        : base(options) 
    { 

    }
    public MikeBerriesDBContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MikeBerriesDBContext).Assembly);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Provider> Providers { get; set; }
}
