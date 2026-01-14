using MediatR;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler(ICartItemRepository cartItemRepository) : IRequestHandler<AddItemToCartCommand, bool>
{
    public async Task<bool> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            UserId = request.UserId,
            Count = request.Count,
            IsChosen = true,
            Cart =  null!
        };
        
        await cartItemRepository.AddAsync(cartItem, cancellationToken);
        
        await cartItemRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
