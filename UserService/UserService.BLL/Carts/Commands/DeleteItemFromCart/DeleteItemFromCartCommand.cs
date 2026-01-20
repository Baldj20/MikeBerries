using MediatR;

namespace UserService.BLL.Carts.Commands.DeleteItemFromCart;

public class DeleteItemFromCartCommand : IRequest<bool>
{
    public required Guid CartItemId { get; set; }
    public required string UserId { get; set; }
}
