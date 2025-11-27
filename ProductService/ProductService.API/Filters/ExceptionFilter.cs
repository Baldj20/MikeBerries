using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductService.BLL;
using ProductService.BLL.Logging;

namespace ProductService.API.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.ExceptionOccured(context.Exception.Message, 
            context.Exception.StackTrace ?? string.Empty);

        context.Result = new ObjectResult(Result.Failure(
            new CustomError("Internal server error"), 500))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
