using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetCartByUserId(string  userId);
}
