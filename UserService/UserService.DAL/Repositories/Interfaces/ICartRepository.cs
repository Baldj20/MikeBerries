using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserId(string  userId);
}
