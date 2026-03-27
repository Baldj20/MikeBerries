using MediatR;
using UserService.API.Exceptions;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdentityId(request.IdentityId)
            ?? throw new NotFoundException("User to delete not found");
        
        await userRepository.Delete(user);
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
