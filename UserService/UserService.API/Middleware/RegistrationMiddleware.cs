using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UserService.BLL.Users.Commands.CreateUser;

namespace UserService.API.Middleware;

public class RegistrationMiddleware(RequestDelegate next, IMediator mediator, IDistributedCache cache)
{
    private readonly IDistributedCache _cache = cache;

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var identityId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email  = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var name = context.User.FindFirst(ClaimTypes.Name)?.Value;
            
            long.TryParse(context.User.FindFirst("iat")?.Value, out long tokenIssuedAt);

            if (!string.IsNullOrEmpty(identityId))
            {
                string cacheKey = $"identity_id:{identityId}";
                
                var last = await _cache.GetAsync(cacheKey);
                long lastIssuedAt = last is not null ? BitConverter.ToInt64(last) : 0;

                if (tokenIssuedAt > lastIssuedAt)
                {
                    var createUserCommand = new CreateUserCommand
                    {
                        IdentityId = identityId,
                        Name = name,
                        Email = email,
                    };
                    
                    await mediator.Send(createUserCommand);
                    
                    await _cache.SetAsync(cacheKey, BitConverter.GetBytes(tokenIssuedAt),
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
                }
            }
        }
        await next.Invoke(context);
    }
}
