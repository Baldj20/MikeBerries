using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Filters.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AddStatusCodeAttribute : TypeFilterAttribute
{
    public AddStatusCodeAttribute() : base(typeof(AddStatusCodeFilter)) { }
}
