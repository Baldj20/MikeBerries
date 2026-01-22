using Moq;
using Shouldly;

namespace UserService.UnitTests.Carts.Tests.Queries;

public class GetUserCartQueryTests : CartMocks
{
    [Fact]
    public async Task GetUserCartQuery_ShouldReturnUserCart()
    {
        //Arrange
        var request = _getUserCartQueryFaker.Generate();
        var cart = _cartFaker.Generate();

        _cartRepositoryMock.Setup(m => m.GetCartByUserId(request.UserId)).ReturnsAsync(cart);
        
        //Act
        var act = await _getUserCartQueryHandler.Handle(request, CancellationToken.None);
        
        //Assert
        act.ShouldBeEquivalentTo(cart);
    }
}
