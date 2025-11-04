using Microsoft.Extensions.DependencyInjection;
using ProductService.DAL;

namespace ProductService.IntegrationTests;

public class BaseTest(ProductServiceWebApplicationFactory factory)
    : IClassFixture<ProductServiceWebApplicationFactory>
{
    protected readonly ProductServiceWebApplicationFactory Factory = factory;
    protected async Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
}
