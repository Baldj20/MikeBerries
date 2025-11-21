using Microsoft.AspNetCore.Mvc;

namespace ProductService.API.Filters.Attributes;

public class AddStatusCodeAttribute : TypeFilterAttribute
{
    public AddStatusCodeAttribute() : base(typeof(AddStatusCodeFilter)) { }
}
