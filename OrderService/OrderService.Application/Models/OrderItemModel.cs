namespace OrderService.Application.Models;

public class OrderItemModel
{
    public required Guid OrderId { get; set; }
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}
