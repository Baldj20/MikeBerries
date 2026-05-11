using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class RedisCacheRepository(IDistributedCache cache) : ICacheRepository
{
    public async Task SetData<T>(string key, T value, 
        DistributedCacheEntryOptions? entryOptions = null, 
        CancellationToken token = default)
    {
        entryOptions ??= new DistributedCacheEntryOptions 
        { 
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) 
        };

        var jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, entryOptions, token);
    }

    public async Task<T?> GetData<T>(string key, CancellationToken token)
    {
        var jsonData = await cache.GetStringAsync(key, token);

        if (string.IsNullOrEmpty(jsonData))
            return default;

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task RemoveData(string key, CancellationToken token)
    {
        await cache.RemoveAsync(key, token);
    }
}
