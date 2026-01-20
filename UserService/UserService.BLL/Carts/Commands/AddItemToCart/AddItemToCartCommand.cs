using MediatR;

namespace UserService.BLL.Carts.Commands.AddItemToCart;

public class AddItemToCartCommand : IRequest<bool>
{
    public required string UserId { get; set; }
    public required Guid ProductId { get; set; }
    public required int Count { get; set; } = 1;
}
