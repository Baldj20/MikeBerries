using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.UserId);

        builder.HasOne(c => c.User)
               .WithOne(u => u.Cart)
               .HasForeignKey<Cart>(c => c.UserId);

        builder.Property(c => c.TotalCount);

        builder.Property(c => c.TotalPrice);

        builder.HasMany(c => c.Items)
               .WithOne(ci => ci.Cart)
               .HasForeignKey(ci => ci.UserId);
    }
}
