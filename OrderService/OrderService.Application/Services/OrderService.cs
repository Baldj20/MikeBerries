using Google.Protobuf.Collections;
using Mapster;
using OrderService.Application.Models;
using OrderService.Application.Services.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces.Repositories;
using Shared.Protos;

namespace OrderService.Application.Services;

public class OrderService(IOrderRepository orderRepository, ProductService.ProductServiceClient grpcClient) : IOrderService
{
    public async Task MakeAnOrder(OrderModel orderModel, CancellationToken cancellationToken)
    {
        var order = orderModel.Adapt<Order>();

        var ids = order.Items.Select(i => i.ProductId.ToString()).ToList();
        
        var request = new ProductsAvailabilityRequest();
        request.ProductId.AddRange(ids);
        
        var response = await grpcClient.AreProductsAvailableAsync(request);
        var isAvailableList = response.IsAvailable;

        foreach (var isAvailable in isAvailableList)
        {
            if (!isAvailable)
            {
                throw new Exception("Product not available");
            }
        }
        
        await orderRepository.AddAsync(order, cancellationToken);
        
        await orderRepository.SaveChangesAsync(cancellationToken);
    }
}
