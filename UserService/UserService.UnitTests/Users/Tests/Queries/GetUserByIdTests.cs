using Moq;
using Shouldly;

namespace UserService.UnitTests.Users.Tests.Queries;

public class GetUserByIdTests : UserMocks
{
    [Fact]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.GetUserByIdentityId(It.IsAny<string>())).ReturnsAsync(_userFaker.Generate());
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _getUserByIdFaker.Generate();
        
        //Act
        var user = await _getUserByIdQueryHandler.Handle(request, CancellationToken.None);
        
        //Assert
        user.ShouldNotBeNull();
    }
}
