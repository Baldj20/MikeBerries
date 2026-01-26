using Moq;
using Shouldly;
using UserService.API.Exceptions;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Users.Tests.Commands;

public class UpdateUserTests : UserMocks
{
    [Fact]
    public async Task UpdateUserIfUserExists_UserReturnShouldNotBeNull()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.GetUserByAuth0Id(It.IsAny<string>())).ReturnsAsync(_userFaker.Generate());
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _updateUserCommandFaker.Generate();
        
        //Act
        var user = await _updateUserCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        user.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task UpdateUserIfUserNotExists_ShouldThrowNotFoundException()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.GetUserByAuth0Id(It.IsAny<string>())).ReturnsAsync((User)null!);
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _updateUserCommandFaker.Generate();
        
        //Act
        var act = () =>  _updateUserCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        await act.ShouldThrowAsync<NotFoundException>();
    }
}
