using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.ProductId);

        builder.HasOne(ci => ci.Cart)
               .WithMany(c => c.Items)
               .HasForeignKey(c => c.UserId);

        builder.Property(ci => ci.Count);

        builder.Property(ci => ci.IsChosen);
    }
}
