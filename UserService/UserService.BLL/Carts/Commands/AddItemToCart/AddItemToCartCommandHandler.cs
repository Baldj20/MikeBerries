using UserService.BLL.Common;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Commands.AddItemToCart;

public class AddItemToCartCommandHandler(IUserRepository userRepository) : IRequestHandler<AddItemToCartCommand, bool>
{
    public async Task<bool> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByAuth0Id(request.UserId);
        if (user is null) return false;

        var cartItem = new CartItem
        {
            Id = Guid.NewGuid(),
            ProductId = request.ProductId,
            UserId = request.UserId,
            Count = request.Count,
            IsChosen = true,
            Cart =  user.Cart
        };
        
        user.Cart.Items.Add(cartItem);
        
        await userRepository.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}
