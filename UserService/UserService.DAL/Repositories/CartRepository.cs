using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.DAL.Repositories;

public class CartRepository(UserServiceDbContext context) : Repository<Cart>(context), ICartRepository
{
    public async Task<Cart?> GetCartByUserId(string userId)
    {
        var cart = await Context.Carts
            .Where(c => c.UserId == userId)
            .Include(c => c.Items)
            .FirstOrDefaultAsync();

        return cart;
    }
}
