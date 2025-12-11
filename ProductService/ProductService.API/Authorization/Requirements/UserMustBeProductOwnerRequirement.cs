using Microsoft.AspNetCore.Authorization;

namespace ProductService.API.Authorization.Requirements;

public class UserMustBeProductOwnerRequirement : IAuthorizationRequirement { }
