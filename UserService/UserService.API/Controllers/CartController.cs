using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.BLL.Carts.Commands.AddItemToCart;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;
using UserService.BLL.Carts.Queries.GetUserCart;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<GetCartDto> GetUserCart(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserCartQuery
        {
            UserId = id
        };
        
        var cart = await mediator.Send(query, cancellationToken);
        
        return mapper.Map<GetCartDto>(cart);
    }
    
    [HttpPost("{cartId}/items")]
    public async Task<ActionStatusDto> AddItemToCart(AddItemDto dto, CancellationToken cancellationToken)
    {
        var command = mapper.Map<AddItemToCartCommand>(dto);
        
        var success = await mediator.Send(command, cancellationToken);

        return new ActionStatusDto
        {
            Status = success,
            Message = success? "Success": "Failure"
        };
    }

    [HttpDelete("{cartId}/items/{itemId}")]
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
            Message = success? "Success": "Failure"
        };
    }
}
