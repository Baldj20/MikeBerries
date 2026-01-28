using Microsoft.Extensions.Options;
using Shared.Protos;

namespace OrderService.Presentation.Configurations;

public static class GrpcConfiguration
{
    public static void AddGrpcConfiguration(this IServiceCollection services,  IConfiguration configuration)
    {
        services.Configure<GrpcSettings>(configuration.GetSection("GrpcSettings"));
        
        services.AddGrpcClient<ProductService.ProductServiceClient>((sp, o) =>
        {
            var settings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
        
            if (settings.ProductServiceUrl is null)
            {
                throw new InvalidOperationException("Grpc settings are not configured!");
            }

            o.Address = new Uri(settings.ProductServiceUrl);
        });
    }
}
