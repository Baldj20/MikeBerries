namespace UserService.BLL.Carts.Commands.DeleteItemFromCart;

public class DeleteItemFromCartCommand
{
    public required Guid CartItemId { get; set; }
    public required string UserId { get; set; }
}