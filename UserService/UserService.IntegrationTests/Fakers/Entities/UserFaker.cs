using Bogus;
using UserService.DAL.Entities;

namespace UserService.IntegrationTests.Fakers.Entities;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(c => c.IdentityId, f => f.Random.Guid().ToString());
        RuleFor(c => c.Email, f => f.Person.Email);
        RuleFor(c => c.Name, f => f.Person.FullName);
        RuleFor(c => c.Cart, new CartFaker().Generate());
    }
}
