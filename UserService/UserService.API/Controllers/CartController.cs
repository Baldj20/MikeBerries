using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.BLL.Carts.Queries.GetUserCart;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<GetCartDto> GetUserCart(string id, CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetUserCartQuery>(id);
        
        var cart = await mediator.Send(query, cancellationToken);
        
        return mapper.Map<GetCartDto>(cart);
    }
}
