namespace OrderService.Domain.Entities;

public class Order
{
    public required Guid Id { get; set; }
    public required string Auth0Id { get; set; }
    public required int TotalPrice { get; set; }
    
    public List<OrderItem> Items { get; set; }
}
