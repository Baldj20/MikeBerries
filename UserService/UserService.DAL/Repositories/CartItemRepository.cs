using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class CartItemRepository(UserServiceDbContext dbContext) : Repository<CartItem>(dbContext), ICartItemRepository
{
    public async Task<CartItem?> GetItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken)
    {
        var cartItem = await Context.CartItems.Where(ci => ci.Id == cartItemId)
            .FirstOrDefaultAsync(cancellationToken);
        
        return cartItem;
    }
}
