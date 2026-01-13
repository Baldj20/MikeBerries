using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DAL.Entities;

namespace UserService.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Auth0Id);

        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.Name);

        builder.HasOne(u => u.Cart)
               .WithOne(c => c.User)
               .HasForeignKey<Cart>(c => c.UserId);
    }
}
