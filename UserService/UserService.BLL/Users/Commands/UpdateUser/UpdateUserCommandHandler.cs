using UserService.BLL.Common;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByAuth0Id(request.Auth0Id);
        
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }
        
        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        await userRepository.SaveChangesAsync(cancellationToken);
        return user;
    }
}
