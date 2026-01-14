using UserService.DAL.Entities;

namespace UserService.DAL.Repositories.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    Task<CartItem?> GetItemByIdAsync(Guid cartItemId, CancellationToken cancellationToken);
}
