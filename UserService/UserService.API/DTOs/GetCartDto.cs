namespace UserService.API.DTOs;

public class GetCartDto
{
    public required List<CartItemDto> Items { get; set; } = new();
    public required int TotalPrice { get; set; }
    public required int TotalCount { get; set; }
}
