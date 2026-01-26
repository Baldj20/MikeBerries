using Bogus;
using UserService.BLL.Users.Queries.GetUserById;

namespace UserService.UnitTests.Users.Fakers.Queries;

public class GetUserByIdFaker : Faker<GetUserByIdQuery>
{
    public GetUserByIdFaker()
    {
        RuleFor(c => c.Auth0Id, f => f.Lorem.Word());
    }
}
