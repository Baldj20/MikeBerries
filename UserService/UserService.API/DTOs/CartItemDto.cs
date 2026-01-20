namespace UserService.API.DTOs;

public class CartItemDto
{
    public required Guid ProductId { get; set; }
    public required int Count { get; set; } = 0;
    public required bool IsChosen { get; set; } = false;
}
