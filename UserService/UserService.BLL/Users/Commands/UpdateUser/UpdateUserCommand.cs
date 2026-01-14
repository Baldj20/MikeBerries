using MediatR;

namespace UserService.BLL.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<bool>
{
    public required string Auth0Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
}
