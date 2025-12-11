using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Filters.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RequireProductOwningAttribute : TypeFilterAttribute
{
    public RequireProductOwningAttribute() : base(typeof(ProductOwnerActionFilter)) { }
}
