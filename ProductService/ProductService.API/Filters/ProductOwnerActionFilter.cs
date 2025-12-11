using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProductService.BLL;
using ProductService.BLL.Constants;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.API.Filters;

public class ProductOwnerActionFilter : IAsyncActionFilter
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductOwnerActionFilter(
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments["id"] is not Guid id)
        {
            context.Result = new ObjectResult(Result.Failure(CustomError.BadRequest("id"), 400))
            {
                StatusCode = 400
            };

            return;
        }

        var product = await _unitOfWork.Products.GetByIdAsync(id, default);

        if (product is null)
        {
            context.Result = new ObjectResult(Result
                .Failure(CustomError.ResourceNotFound<Product>(), 404));

            return;
        }

        var authResult = await _authorizationService.AuthorizeAsync(
            context.HttpContext.User,
            product,
            PoliciesNames.USER_MUST_BE_PRODUCT_OWNER
        );

        if (!authResult.Succeeded)
        {
            context.Result = new ObjectResult(Result.Failure(CustomError.ResourceForbidden(), 403))
            {
                StatusCode = 403
            };

            return;
        }

        await next();
    }
}
