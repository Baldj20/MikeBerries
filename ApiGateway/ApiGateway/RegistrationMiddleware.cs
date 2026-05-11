using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiGateway;

public class RegistrationMiddleware(IHttpClientFactory clientFactory, IDistributedCache cache, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                     throw new ArgumentNullException(ClaimTypes.NameIdentifier);

        var cacheKey = $"userId:{userId}";

        var isRegistered = await cache.GetStringAsync(cacheKey);

        if (isRegistered is not null)
        {
            await next.Invoke(context);
        }
        else
        {
            var email =  context.User.FindFirstValue(ClaimTypes.Email) ??
                         throw new ArgumentNullException(ClaimTypes.NameIdentifier);;
            var name = context.User.FindFirstValue(ClaimTypes.Name) ??
                       throw new ArgumentNullException(ClaimTypes.NameIdentifier);
            
            var client = clientFactory.CreateClient();

            var payload = new
            {
                IdentityId = userId,
                Name = name,
                Email = email
            };
            
            var response = await client.PostAsJsonAsync("http://user-service:8080/api/users", payload);
            
            await cache.SetStringAsync(cacheKey, "true", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            });
        }
    }
}
