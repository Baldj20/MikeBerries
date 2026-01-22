using Bogus;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Common.Fakers;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(c => c.Auth0Id, f => f.Lorem.Word());
        RuleFor(c => c.Email, f => f.Person.Email);
        RuleFor(c => c.Name, f => f.Person.FullName);
        RuleFor(c => c.Cart, new CartFaker().Generate());
    }
}
