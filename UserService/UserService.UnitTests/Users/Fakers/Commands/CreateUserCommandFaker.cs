using Bogus;
using UserService.BLL.Users.Commands.CreateUser;

namespace UserService.UnitTests.Users.Fakers.Commands;

public class CreateUserCommandFaker : Faker<CreateUserCommand>
{
    public CreateUserCommandFaker()
    {
        RuleFor(c => c.IdentityId, f => f.Lorem.Word());
        RuleFor(c => c.Name, f => f.Lorem.Word());
        RuleFor(c => c.Email, f => f.Lorem.Word());
    }
}
