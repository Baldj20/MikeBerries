using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Exceptions;

namespace UserService.API;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "{Exception}: {Message}", ExceptionConstants.NOT_FOUND_EXCEPTION_MESSAGE, ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.NotFound);
        }
        catch (BadRequestException ex)
        {
            logger.LogWarning(ex, "{Exception}: {Message}", ExceptionConstants.BAD_REQUEST_EXCEPTION_MESSAGE, ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Exception}: {Message}", ExceptionConstants.UNHANDLED_EXCEPTION_MESSAGE, ex.Message);
            await WriteErrorResponse(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static async Task WriteErrorResponse(
        HttpContext context,
        Exception exception,
        HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = (int)statusCode,
            Detail = exception.Message,
        };
        
        await context.Response.WriteAsJsonAsync(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
