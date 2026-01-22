using Moq;
using Shouldly;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Carts.Tests.Commands;

public class AddItemToCartTests : CartMocks
{
    [Fact]
    public async Task AddItemToCart_ShouldReturnTrue()
    {
        //Arrange
        _cartItemRepositoryMock.Setup(m => m.AddAsync(It.IsAny<CartItem>(), CancellationToken.None)).Returns(Task.CompletedTask);
        _cartItemRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _addItemToCartCommandFaker.Generate();
        
        //Act
        var success = await _addItemToCartCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        success.ShouldBeTrue();
    }
}
