using MediatR;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Commands.DeleteItemFromCart;

public class DeleteItemFromCartCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteItemFromCartCommand, bool>
{
    public async Task<bool> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByAuth0Id(request.UserId);

        if (user is null)
        {
            return false;
        }
        
        user.Cart.Items.RemoveAll(ci => ci.Id == request.CartItemId);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
