using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.DAL.Entities;

namespace ProductService.DAL.Configurations;

internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasOne(i => i.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(i => i.ProductId);

        builder.Property(i => i.Url);
    }
}
