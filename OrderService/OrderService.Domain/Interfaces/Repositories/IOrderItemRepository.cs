using OrderService.Domain.Entities;

namespace OrderService.Domain.Interfaces.Repositories;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<OrderItem?> GetByIdAsync(Guid orderId, Guid productId, CancellationToken token);
}
