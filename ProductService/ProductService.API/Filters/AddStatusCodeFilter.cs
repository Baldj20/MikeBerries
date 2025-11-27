using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductService.BLL;

namespace ProductService.API.Filters;

public class AddStatusCodeFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context) { }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult &&
            objectResult.Value is Result result)
        {
            objectResult.StatusCode = result.StatusCode;
        }
    }
}
