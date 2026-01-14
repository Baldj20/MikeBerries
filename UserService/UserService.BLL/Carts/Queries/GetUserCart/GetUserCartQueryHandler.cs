using MediatR;
using UserService.DAL.Entities;
using UserService.DAL.Repositories.Interfaces;

namespace UserService.BLL.Carts.Queries.GetUserCart;

public class GetUserCartQueryHandler(ICartRepository cartRepository) : IRequestHandler<GetUserCartQuery, Cart?>
{
    public async Task<Cart?> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetCartByUserId(request.UserId);
        
        return cart;
    }
}
