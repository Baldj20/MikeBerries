namespace OrderService.Domain.Entities;

public class OrderItem
{
    public required Guid OrderId { get; set; }
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
    
    public Order? Order { get; set; }
}
