using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.BLL;
using ProductService.DAL;

namespace ProductService.IntegrationTests;

public class BaseTest
    : IClassFixture<ProductServiceWebApplicationFactory>, IAsyncLifetime
{
    protected readonly ProductServiceWebApplicationFactory Factory;
    private readonly IServiceScope Scope;
    private readonly MikeBerriesDBContext DbContext;
    public BaseTest(ProductServiceWebApplicationFactory factory)
    {
        Factory = factory;
        Scope = Factory.Services.CreateScope();
        DbContext = Scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();
    }
    protected async Task<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
    {
        DbContext.Add(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }
    protected async Task AddRange<TEntity>(IEnumerable<TEntity> range) where TEntity : class
    {
        DbContext.AddRange(range);
        await DbContext.SaveChangesAsync();
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
    public Task InitializeAsync()
    {
        DbContext.Database.EnsureCreated();

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        DbContext.Database.EnsureDeleted();
        Scope.Dispose();

        return Task.CompletedTask;
    }
}
