using Microsoft.AspNetCore.Authorization;
using ProductService.API.Authorization.Requirements;
using ProductService.API.Constants;
using ProductService.DAL.Entities;
using System.Security.Claims;

namespace ProductService.API.Authorization.Handlers;

public class AuthorizationHandler : AuthorizationHandler<UserMustBeProductOwnerRequirement, Product>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
        UserMustBeProductOwnerRequirement requirement, Product resource)
    {
        if (context.User.IsInRole(RolesNames.ADMIN))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString))
        {
            return Task.CompletedTask;
        }

        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Task.CompletedTask;
        }

        if (resource.ProviderId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
