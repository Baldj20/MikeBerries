using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

public class OrderItemRepository(OrderServiceDbContext context) : Repository<OrderItem>(context), IOrderItemRepository
{
    public async Task<OrderItem?> GetByIdAsync(Guid orderId, Guid productId, CancellationToken token)
    {
        var orderItem = await Context.OrderItems.Where(oi => oi.OrderId == orderId && oi.ProductId == productId)
            .FirstOrDefaultAsync(token);

        return orderItem;
    }
}
