namespace UserService.DAL.Entities;

public class CartItem
{
    public required Guid Id { get; set; }
    public required Guid ProductId { get; set; }
    public required string UserId { get; set; }
    public required int Count { get; set; } = 0;
    public required bool IsChosen { get; set; } = false;

    public required Cart Cart { get; set; }
}
