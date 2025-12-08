using Microsoft.Extensions.Caching.Distributed;

namespace ProductService.DAL.Interfaces.Repositories;

public interface ICacheRepository
{
    Task SetData<T>(string key, T value, 
        DistributedCacheEntryOptions? entryOptions = null, 
        CancellationToken token = default);
    Task<T?> GetData<T>(string key, CancellationToken token);
    Task RemoveData(string key, CancellationToken token);
}
