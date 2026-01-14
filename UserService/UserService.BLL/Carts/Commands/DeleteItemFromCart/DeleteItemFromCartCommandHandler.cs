using MediatR;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Commands.DeleteItemFromCart;

public class DeleteItemFromCartCommandHandler(ICartItemRepository cartItemRepository) : IRequestHandler<DeleteItemFromCartCommand, bool>
{
    public async Task<bool> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await cartItemRepository.GetItemByIdAsync(request.CartItemId, cancellationToken);
        
        if (cartItem == null) return false;
        
        await cartItemRepository.Delete(cartItem);
        
        await cartItemRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
