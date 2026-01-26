using Moq;
using Shouldly;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Users.Tests.Commands;

public class CreateUserTests : UserMocks
{
    [Fact]
    public async Task CreateUser_ShouldNotReturnNull()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.AddAsync(It.IsAny<User>(), CancellationToken.None)).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(m => m.SaveChangesAsync(CancellationToken.None)).Returns(Task.CompletedTask);
 
        var request = _createUserCommandFaker.Generate();
        
        //Act
        var user = await _createUserCommandHandler.Handle(request, CancellationToken.None);
        
        //Assert
        user.ShouldNotBeNull();
    }
}
