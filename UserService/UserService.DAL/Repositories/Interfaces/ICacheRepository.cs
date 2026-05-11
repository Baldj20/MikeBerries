using Microsoft.Extensions.Caching.Distributed;

namespace UserService.DAL.Repositories.Interfaces;

public interface ICacheRepository
{
    Task SetData<T>(string key, T value, 
        DistributedCacheEntryOptions? entryOptions = null, 
        CancellationToken token = default);
    Task<T?> GetData<T>(string key, CancellationToken token);
    Task RemoveData(string key, CancellationToken token);
}
