using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Filters.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RequireProductOwnnessAttribute : TypeFilterAttribute
{
    public RequireProductOwnnessAttribute() : base(typeof(ProductOwnerActionFilter)) { }
}
