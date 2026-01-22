using Moq;
using Shouldly;
using UserService.API.Exceptions;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Users.Tests.Commands;

public class DeleteUserTests : UserMocks
{
    [Fact]
    public async Task DeleteUserIfUserExists_ShouldReturnTrue()
    {
        //Arrange
        var user = _userFaker.Generate();
        _userRepositoryMock.Setup(m => m.GetUserByAuth0Id(It.IsAny<string>())).ReturnsAsync(user);
        _userRepositoryMock.Setup(m => m.Delete(user)).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _deleteUserCommandFaker.Generate();
        
        //Act
        var success = await _deleteUserCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        success.ShouldBeTrue();
    }
    
    [Fact]
    public async Task DeleteUserIfUserNotExists_ShouldReturnFalse()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.GetUserByAuth0Id(It.IsAny<string>())).ReturnsAsync((User)null!);
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _deleteUserCommandFaker.Generate();
        
        //Act & Assert
        await Should.ThrowAsync<NotFoundException>(async () =>
        {
            await _deleteUserCommandHandler.Handle(request, CancellationToken.None);
        });
    }
}
