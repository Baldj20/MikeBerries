using MediatR;
using UserService.DAL.Entities;

namespace UserService.BLL.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<User>
{
    public required string Auth0Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
}
