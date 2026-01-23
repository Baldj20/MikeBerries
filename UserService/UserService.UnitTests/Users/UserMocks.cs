using Moq;
using UserService.BLL.Users.Commands.CreateUser;
using UserService.BLL.Users.Commands.DeleteUser;
using UserService.BLL.Users.Commands.UpdateUser;
using UserService.BLL.Users.Queries.GetUserById;
using UserService.DAL.Repositories.Interfaces;
using UserService.UnitTests.Common.Fakers;
using UserService.UnitTests.Users.Fakers.Commands;
using UserService.UnitTests.Users.Fakers.Queries;

namespace UserService.UnitTests.Users;

public class UserMocks
{
    protected readonly Mock<IUserRepository> _userRepositoryMock;
    
    protected readonly CreateUserCommandHandler _createUserCommandHandler;
    protected readonly DeleteUserCommandHandler _deleteUserCommandHandler;
    protected readonly UpdateUserCommandHandler _updateUserCommandHandler;
    
    protected readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;
    
    protected readonly CreateUserCommandFaker _createUserCommandFaker;
    protected readonly DeleteUserCommandFaker _deleteUserCommandFaker;
    protected readonly UpdateUserCommandFaker _updateUserCommandFaker;
    
    protected readonly GetUserByIdFaker _getUserByIdFaker;
    
    protected readonly UserFaker _userFaker;
    
    protected UserMocks()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        
        _createUserCommandHandler = new CreateUserCommandHandler(_userRepositoryMock.Object);
        _deleteUserCommandHandler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
        _updateUserCommandHandler = new UpdateUserCommandHandler(_userRepositoryMock.Object);
        
        _getUserByIdQueryHandler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
        
        _createUserCommandFaker = new CreateUserCommandFaker();
        _deleteUserCommandFaker = new DeleteUserCommandFaker();
        _updateUserCommandFaker = new UpdateUserCommandFaker();
        
        _getUserByIdFaker = new GetUserByIdFaker();
        
        _userFaker = new UserFaker();
    }
}
