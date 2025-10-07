using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.DAL.Entities;

namespace ProductService.DAL.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Article);

        builder.HasOne(prod => prod.Provider)
               .WithMany(prov => prov.Products)
               .HasForeignKey(prod => prod.ProviderEmail);

        builder.HasMany(prod => prod.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductArticle);

        builder.Property(p => p.Description);

        builder.Property(p => p.Price);
    }
}
