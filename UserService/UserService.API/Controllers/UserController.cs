using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs;
using UserService.BLL.Users.Commands.DeleteUser;
using UserService.BLL.Users.Commands.UpdateUser;
using UserService.BLL.Users.Queries.GetUserById;

namespace UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<GetUserDto> GetUserById(string id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery
        {
            Auth0Id = id
        };
        
        var user = await mediator.Send(query, cancellationToken);
        
        return mapper.Map<GetUserDto>(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionStatusDto> DeleteUserById(string id, CancellationToken cancellationToken)
    {
        var command = mapper.Map<DeleteUserCommand>(id);
        
        var success = await mediator.Send(command, cancellationToken);
        
        return new ActionStatusDto
        {
            Status = success,
            Message = success ? string.Empty : "Delete failed."
        };
    }

    [HttpPut("{id}")]
    public async Task<GetUserDto> UpdateUserById(string id, UpdateUserDto dto, CancellationToken cancellationToken)
    {
        dto.Auth0Id = id;
        
        var command = mapper.Map<UpdateUserCommand>(dto);

        var user = await mediator.Send(command, cancellationToken);

        return new GetUserDto
        {
            Name = user.Name,
            Email = user.Email,
        };
    }
}
