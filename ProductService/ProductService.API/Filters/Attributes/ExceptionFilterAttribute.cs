using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Filters.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ExceptionFilterAttribute : TypeFilterAttribute
{
    public ExceptionFilterAttribute() : base(typeof(ExceptionFilterAttribute)) { }
}
