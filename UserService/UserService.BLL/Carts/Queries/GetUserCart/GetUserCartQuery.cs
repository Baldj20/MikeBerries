using MediatR;
using UserService.DAL.Entities;

namespace UserService.BLL.Carts.Queries.GetUserCart;

public class GetUserCartQuery : IRequest<Cart>
{
    public required string UserId { get; set; }
}
