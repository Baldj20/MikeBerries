using UserService.BLL.Common;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByAuth0Id(request.Auth0Id);
        if (user is null)
        {
            return false;
        }
        
        await userRepository.Delete(user);
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
