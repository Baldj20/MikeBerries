using OrderService.Application.Models;

namespace OrderService.Application.Services.Interfaces;

public interface IOrderService
{
    Task MakeAnOrder(OrderModel order, CancellationToken cancellationToken);
}
