using Mapster;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using Polly.Retry;
using ProductService.API.DTO;
using ProductService.API.Resilience;
using ProductService.BLL.DTO;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

namespace ProductService.API;

public static class ApiExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MikeBerriesDBContext>();

            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(context.Database.Migrate);
        }
    }

    public static void ConfigureMapping()
    {
        TypeAdapterConfig<IFormFile, IFormFile>.NewConfig().MapWith(src => src);
        TypeAdapterConfig<CreateProductDto, ProductModel>.NewConfig()
            .Ignore(dest => dest.Images)
            .AfterMapping((dto, model) =>
            {
                foreach (var image in dto.Images)
                {
                    var imageModel = new ProductImageModel
                    {
                        Image = image,
                        Product = null!
                    };

                    model.Images.Add(imageModel);
                }
            });
        TypeAdapterConfig<Product, ProductModel>.NewConfig()
            .Ignore(d => d.Provider.Products);
        TypeAdapterConfig<UpdateImageDto, UpdateImageModel>.NewConfig();
        TypeAdapterConfig<UpdateProductDto, UpdateProductModel>.NewConfig();
        TypeAdapterConfig<UpdateProductModel, Product>.NewConfig()
            .IgnoreNullValues(true)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Images);

        TypeAdapterConfig<ProductImage, ProductImageModel>.NewConfig()
            .Ignore(d => d.Product.Images)
            .Ignore(d => d.Product.Provider.Products);
        TypeAdapterConfig<ProductImageModel, ProductImage>.NewConfig()
            .Ignore(dest => dest.Product);
    }

    public static void AddResiliencePipeline(this IServiceCollection services, 
        string pipelineName, 
        IConfiguration configuration, 
        string configSectionName = ResilienceOptions.CONFIG_SECTION_NAME)
    {
        var options = configuration.GetSection(configSectionName).Get<ResilienceOptions>()
            ?? new ResilienceOptions();

        services.AddResiliencePipeline(pipelineName, builder =>
        {
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = options.Retry.MaxRetryAttempts,
                Delay = TimeSpan.FromMilliseconds(options.Retry.DelayMilliseconds),
                BackoffType = DelayBackoffType.Exponential,
            });

            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = options.CircuitBreaker.FailureRatio,
                SamplingDuration = TimeSpan.FromSeconds(options.CircuitBreaker.SamplingDurationSeconds),
                MinimumThroughput = options.CircuitBreaker.MinimumThroughput,
                BreakDuration = TimeSpan.FromSeconds(options.CircuitBreaker.BreakDurationSeconds)
            });
        });
    }

    public static void AddMinio(this IServiceCollection services, IConfiguration configuration)
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
}
