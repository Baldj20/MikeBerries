using MediatR;
using UserService.DAL.Entities;

namespace UserService.BLL.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<User>
{
    public required string IdentityId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
}
