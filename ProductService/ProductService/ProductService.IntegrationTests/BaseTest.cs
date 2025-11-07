using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.BLL;
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
    public Task InitializeAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return Task.CompletedTask;
    }
    protected async Task AddRange<TEntity>(IEnumerable<TEntity> range) where TEntity : class
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();
        dbContext.AddRange(range);
        await dbContext.SaveChangesAsync();
    }
    protected static async Task<Result?> DeserializeResponse(HttpResponseMessage response)
    {
        var serializedContent = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonConvert.DeserializeObject<Result>(serializedContent);
        return deserializedResult;
    }
    protected static async Task<Result<T>?> DeserializeResponseTo<T>(HttpResponseMessage response)
    {
        var serializedContent = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonConvert.DeserializeObject<Result<T>>(serializedContent);
        return deserializedResult;
    }
}
