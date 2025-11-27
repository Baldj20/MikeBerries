using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.DAL.Repositories;

public class RedisCacheRepository(IDistributedCache cache) : ICacheRepository
{
    private readonly MessagePackSerializerOptions options =
        MessagePackSerializerOptions.Standard.WithResolver(CompositeResolver.Create(
            NativeDateTimeResolver.Instance,
            StandardResolver.Instance,
            ContractlessStandardResolver.Instance
        ))
        .WithSecurity(MessagePackSecurity.UntrustedData);
    public async Task<T?> GetData<T>(string key, CancellationToken token)
    {
        var bytes = await cache.GetAsync(key, token);

        if (bytes is null || bytes.Length == 0) return default;

        return MessagePackSerializer.Deserialize<T>(bytes, options);
    }

    public async Task RemoveData(string key, CancellationToken token)
    {
        await cache.RemoveAsync(key, token);
    }

    public async Task SetData<T>(string key, T value, 
        DistributedCacheEntryOptions? entryOptions = null, 
        CancellationToken token = default)
    {
        var finalOptions = entryOptions ?? new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        var bytes = MessagePackSerializer.Serialize(value, options);

        await cache.SetAsync(key, bytes, finalOptions, token);
    }
}
