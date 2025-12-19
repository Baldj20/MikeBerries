namespace UserService.DAL.Entities;

public class Cart
{
    public required string UserId { get; set; }
    public required List<CartItem> Items { get; set; } = new();
    public required int TotalPrice { get; set; }
    public required int TotalCount { get; set; }

    public required User User { get; set; }
}
