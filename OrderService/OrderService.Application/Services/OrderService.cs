using Mapster;
using OrderService.Application.Models;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces.Repositories;

namespace OrderService.Application.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public async Task MakeAnOrder(OrderModel orderModel, CancellationToken cancellationToken)
    {
        var order = orderModel.Adapt<Order>();
        
        await orderRepository.AddAsync(order, cancellationToken);
        
        await orderRepository.SaveChangesAsync(cancellationToken);
    }
}
