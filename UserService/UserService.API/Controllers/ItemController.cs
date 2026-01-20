using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.BLL.Carts.Commands.AddItemToCart;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;

namespace UserService.API.Controllers;

[Route("api/carts/{cartId}/[controller]")]
[ApiController]
public class ItemsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<ActionStatusDto> AddItemToCart(AddItemDto dto, CancellationToken cancellationToken)
    {
        var command = mapper.Map<AddItemToCartCommand>(dto);
        
        var success = await mediator.Send(command, cancellationToken);

        return new ActionStatusDto
        {
            Status = success,
            Message = success? "Success": "Lox"
        };
    }

    [HttpDelete("{itemId}")]
    public async Task<ActionStatusDto> DeleteItemFromCart(string cartId, Guid itemId, CancellationToken cancellationToken)
    {
        var command = new DeleteItemFromCartCommand
        {
            CartItemId = itemId,
            UserId = cartId
        };
        
        var success = await mediator.Send(command, cancellationToken);

        return new ActionStatusDto
        {
            Status = success,
            Message = success? "Success": "Lox"
        };
    }
}
