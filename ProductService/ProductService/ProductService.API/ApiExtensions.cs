using Microsoft.EntityFrameworkCore;
using ProductService.DAL;

namespace ProductService.API;

public static class ApiExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();

            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(context.Database.Migrate);
        }
    }
}
