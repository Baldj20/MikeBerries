using Bogus;
using UserService.BLL.Users.Commands.DeleteUser;

namespace UserService.UnitTests.Users.Fakers.Commands;

public class DeleteUserCommandFaker : Faker<DeleteUserCommand>
{
    public DeleteUserCommandFaker()
    {
        RuleFor(c => c.Auth0Id, f => f.Lorem.Word());
    }
}
