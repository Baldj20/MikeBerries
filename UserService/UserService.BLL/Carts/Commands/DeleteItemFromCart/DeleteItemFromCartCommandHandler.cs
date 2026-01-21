using MediatR;
using UserService.API.Exceptions;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Commands.DeleteItemFromCart;

public class DeleteItemFromCartCommandHandler(ICartItemRepository cartItemRepository) : IRequestHandler<DeleteItemFromCartCommand, bool>
{
    public async Task<bool> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await cartItemRepository.GetItemByIdAsync(request.CartItemId, cancellationToken)
            ?? throw new NotFoundException("Cart item to delete not found");
        
        await cartItemRepository.Delete(cartItem);
        
        await cartItemRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
