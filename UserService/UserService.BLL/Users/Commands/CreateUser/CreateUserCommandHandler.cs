using UserService.BLL.Common;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Auth0Id = request.Auth0Id,
            Name = request.Name,
            Email = request.Email,
            Cart = new Cart
            {
                UserId = request.Auth0Id,
                Items = new List<CartItem>(),
                TotalCount = 0,
                TotalPrice = 0,
            }
        };
        
        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);
        return true;    
    }
}