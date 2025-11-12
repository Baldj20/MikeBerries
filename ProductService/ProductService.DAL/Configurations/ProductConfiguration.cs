using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.DAL.Entities;

namespace ProductService.DAL.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(prod => prod.Provider)
               .WithMany(prov => prov.Products)
               .HasForeignKey(prod => prod.ProviderId);

        builder.HasMany(prod => prod.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId);

        builder.Property(p => p.Description);

        builder.Property(p => p.Price)
               .IsRequired();

        builder.Property(p => p.Title)
               .IsRequired();
    }
}
