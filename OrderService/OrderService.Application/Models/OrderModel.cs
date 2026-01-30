namespace OrderService.Application.Models;

public class OrderModel
{
    public required Guid Id { get; set; }
    public required string Auth0Id { get; set; }
    public required int TotalPrice { get; set; }
    public required List<OrderItemModel> Items { get; set; }
    public required DateTime CreatedAt { get; set; }
}
