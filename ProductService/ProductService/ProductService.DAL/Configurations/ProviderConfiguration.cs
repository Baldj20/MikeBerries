using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.DAL.Entities;

namespace ProductService.DAL.Configurations;

internal class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasMany(prov => prov.Products)
               .WithOne(prod => prod.Provider)
               .HasForeignKey(prod => prod.ProviderId);

        builder.Property(p => p.Name);

        builder.HasIndex(p => p.Email)
               .IsUnique();
    }
}
