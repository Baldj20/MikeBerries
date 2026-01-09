using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;
using StackExchange.Redis;

namespace ProductService.DAL.Configurations;

public static class DataAccessLayerExtention
{
    public static void ConfigureDataAccessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<MikeBerriesDBContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMinio(configuration);
        services.AddRedis(configuration);
    }
    private static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var minioSettings = configuration.GetSection(MinioSettings.CONFIG_SECTION_NAME)
                                         .Get<MinioSettings>();

        if (minioSettings is null) throw new InvalidOperationException("Minio settings not found");

        services.AddSingleton(sp =>
        {
            var pipelineProvider = sp.GetRequiredService<ResiliencePipelineProvider<string>>();
            return new MinioStorage(minioSettings, pipelineProvider);
        });

        services.AddScoped<IFileRepository, MinioFileRepository>();
    }
    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
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
