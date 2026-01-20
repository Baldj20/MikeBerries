using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.BLL.Carts.Commands.AddItemToCart;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
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

    [HttpDelete("{id}")]
    public async Task<ActionStatusDto> DeleteItemFromCart(string id, DeleteItemFromCartDto dto, CancellationToken cancellationToken)
    {
        var command = mapper.Map<DeleteItemFromCartCommand>(dto);
        
        var success = await mediator.Send(command, cancellationToken);

        return new ActionStatusDto
        {
            Status = success,
            Message = success? "Success": "Lox"
        };
    }
}
