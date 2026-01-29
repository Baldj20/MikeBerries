using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository(OrderServiceDbContext context) : Repository<Order>(context), IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken token)
    {
        var order = await Context.Orders.Where(o => o.Id == id)
            .FirstOrDefaultAsync(token);

        return order;
    }
}
