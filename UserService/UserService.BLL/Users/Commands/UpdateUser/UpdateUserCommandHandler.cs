using MediatR;
using UserService.API.Exceptions;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdentityId(request.IdentityId)
            ?? throw new NotFoundException("User to update not found");
        
        user.Name = request.Name ?? user.Name;
        user.Email = request.Email ?? user.Email;
        await userRepository.SaveChangesAsync(cancellationToken);
        return user;
    }
}
