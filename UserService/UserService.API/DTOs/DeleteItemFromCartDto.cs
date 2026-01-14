namespace UserService.API.DTOs;

public class DeleteItemFromCartDto
{
    public required string CartId { get; set; }
    public required Guid ItemId { get; set; }
}
