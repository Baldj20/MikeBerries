using Medallion.Threading;
using Medallion.Threading.Redis;
using StackExchange.Redis;
using UserService.DAL.Repositories;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.API.Configurations;

public static class Extensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");

        if(redisConnectionString is null) throw new InvalidOperationException("Redis connection string is not found");

        var connection = ConnectionMultiplexer.Connect(redisConnectionString);

        services.AddSingleton<IDistributedLockProvider>(_ =>
            new RedisDistributedSynchronizationProvider(connection.GetDatabase()));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddScoped<ICacheRepository, RedisCacheRepository>();
    }
}
