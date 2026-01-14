using MediatR;

namespace UserService.BLL.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public required string Auth0Id { get; set; }
}
