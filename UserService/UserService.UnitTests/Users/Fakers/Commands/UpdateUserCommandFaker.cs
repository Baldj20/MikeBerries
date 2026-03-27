using Bogus;
using UserService.BLL.Users.Commands.UpdateUser;

namespace UserService.UnitTests.Users.Fakers.Commands;

public class UpdateUserCommandFaker : Faker<UpdateUserCommand>
{
    public UpdateUserCommandFaker()
    {
        RuleFor(c => c.IdentityId, f => f.Lorem.Word());
        RuleFor(c => c.Email, f => f.Person.Email);
        RuleFor(c => c.Name, f => f.Person.FullName);
    }
}
