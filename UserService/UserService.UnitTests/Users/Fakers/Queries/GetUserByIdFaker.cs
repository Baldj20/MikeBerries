using Bogus;
using UserService.BLL.Users.Queries.GetUserById;

namespace UserService.UnitTests.Users.Fakers.Queries;

public class GetUserByIdFaker : Faker<GetUserByIdQuery>
{
    public GetUserByIdFaker()
    {
        RuleFor(c => c.IdentityId, f => f.Lorem.Word());
    }
}
