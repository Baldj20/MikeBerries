using UserService.BLL.Common;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByAuth0Id(request.Auth0Id);
        if (user is null) return false;
        
        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}