using MediatR;

namespace UserService.BLL.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<bool>
{
    public required string Auth0Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
}
