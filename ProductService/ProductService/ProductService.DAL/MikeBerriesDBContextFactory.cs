using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductService.DAL;

internal class MikeBerriesDBContextFactory : IDesignTimeDbContextFactory<MikeBerriesDBContext>
{
    public MikeBerriesDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MikeBerriesDBContext>();

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<MikeBerriesDBContextFactory>()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new MikeBerriesDBContext(optionsBuilder.Options);
    }
}
