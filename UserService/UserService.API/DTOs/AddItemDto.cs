namespace UserService.API.DTOs;

public class AddItemDto
{
    public required Guid ProductId { get; set; }
    public required int Count { get; set; }
}
