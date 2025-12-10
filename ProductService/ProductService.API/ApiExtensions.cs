using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using ProductService.API.Authorization.Handlers;
using ProductService.API.Authorization.Requirements;
using ProductService.API.Constants;
using ProductService.API.DTO;
using ProductService.API.Resilience;
using ProductService.BLL.Constants;
using ProductService.BLL.DTO;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;

namespace ProductService.API;

public static class ApiExtensions
{
    public static void ConfigureAutentication(this IServiceCollection services, IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(AuthSettings.CONFIG_SECTION_NAME).Get<AuthSettings>();
        if (authSettings is null) throw new InvalidOperationException("Authentication settings not found");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = authSettings.Authority;
            options.Audience = authSettings.Audience;
        });
    }
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

        TypeAdapterConfig<ProviderModel, Provider>.NewConfig()
            .Ignore(p => p.Id);
        TypeAdapterConfig<ProductModel, Product>.NewConfig()
            .Ignore(p => p.Id);
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

    public static void AddCachingResiliencePipeline(this IServiceCollection services, string pipelineName
        )
    {
        services.AddResiliencePipeline<string, object?>(pipelineName, builder =>
        {
            builder.AddFallback(new FallbackStrategyOptions<object?>
            {
                ShouldHandle = new PredicateBuilder<object?>().Handle<Exception>(),
                FallbackAction = _ => Outcome.FromResultAsValueTask<object?>(null)
            });

            builder.AddRetry(new RetryStrategyOptions<object?>
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(100),
                BackoffType = DelayBackoffType.Constant
            });

            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<object?>
            {
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30)
            });
        });
    }

    public static void AddPolicyBasedAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PoliciesNames.USER_MUST_BE_PRODUCT_OWNER, policy =>
                policy.Requirements.Add(new UserMustBeProductOwnerRequirement()));

            options.AddPolicy(PoliciesNames.ADMIN, policy => policy.RequireRole(RolesNames.ADMIN));
        });
    }
}
