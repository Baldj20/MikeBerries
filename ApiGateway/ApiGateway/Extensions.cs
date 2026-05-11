namespace ApiGateway;

public static class Extensions
{
    public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");

        if(redisConnectionString is null) throw new InvalidOperationException("Redis connection string is not found");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });
    }
}
