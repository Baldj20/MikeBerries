using Moq;
using Shouldly;
using UserService.API.Exceptions;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Carts.Tests.Commands;

public class DeleteItemFromCartTests : CartMocks
{
    [Fact]
    public async Task DeleteItemFromCartIfCartItemExists_ShouldReturnTrue()
    {
        //Arrange
        var request = _deleteItemFromCartCommandFaker.Generate();

        _cartItemRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        _cartItemRepositoryMock.Setup(m => m.GetItemByIdAsync(request.CartItemId, CancellationToken.None)).ReturnsAsync(_cartItemFaker.Generate());
        
        //Act
        var act = await _deleteItemFromCartCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        act.ShouldBeTrue();
    }
    
    [Fact]
    public async Task DeleteItemFromCartIfCartItemNotExists_ShouldThrowNotFoundException()
    {
        //Arrange
        var request = _deleteItemFromCartCommandFaker.Generate();

        _cartItemRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
        _cartItemRepositoryMock.Setup(m => m.GetItemByIdAsync(request.CartItemId, CancellationToken.None)).ReturnsAsync((CartItem)null!);
        
        //Act
        var act = () =>   _deleteItemFromCartCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }
}
