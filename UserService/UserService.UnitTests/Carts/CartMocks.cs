using Moq;
using UserService.BLL.Carts.Commands.AddItemToCart;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;
using UserService.BLL.Carts.Queries.GetUserCart;
using UserService.DAL.Repositories.Interfaces;
using UserService.UnitTests.Carts.Fakers.Commands;
using UserService.UnitTests.Carts.Fakers.Queries;
using UserService.UnitTests.Common.Fakers;

namespace UserService.UnitTests.Carts;

public class CartMocks
{
    protected readonly Mock<ICartItemRepository> _cartItemRepositoryMock;
    protected readonly Mock<ICartRepository> _cartRepositoryMock;
    
    protected readonly AddItemToCartCommandHandler _addItemToCartCommandHandler;
    protected readonly DeleteItemFromCartCommandHandler _deleteItemFromCartCommandHandler;
    protected readonly GetUserCartQueryHandler _getUserCartQueryHandler;
    
    protected readonly AddItemToCartCommandFaker _addItemToCartCommandFaker;
    protected readonly DeleteItemFromCartCommandFaker _deleteItemFromCartCommandFaker;
    protected readonly GetUserCartQueryFaker _getUserCartQueryFaker;
    protected readonly CartItemFaker _cartItemFaker;
    protected readonly CartFaker _cartFaker;
    
    protected CartMocks()
    {
        _cartItemRepositoryMock = new Mock<ICartItemRepository>();
        _cartRepositoryMock = new Mock<ICartRepository>();
        
        _addItemToCartCommandHandler = new AddItemToCartCommandHandler(_cartItemRepositoryMock.Object);
        _deleteItemFromCartCommandHandler = new DeleteItemFromCartCommandHandler(_cartItemRepositoryMock.Object);
        _getUserCartQueryHandler = new GetUserCartQueryHandler(_cartRepositoryMock.Object);
        
        _addItemToCartCommandFaker = new AddItemToCartCommandFaker();
        _deleteItemFromCartCommandFaker = new DeleteItemFromCartCommandFaker();
        _getUserCartQueryFaker = new GetUserCartQueryFaker();
        _cartItemFaker = new CartItemFaker();
        _cartFaker = new CartFaker();
    }
}
