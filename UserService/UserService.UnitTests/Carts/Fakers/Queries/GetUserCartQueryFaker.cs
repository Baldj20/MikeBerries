using Bogus;
using UserService.BLL.Carts.Queries.GetUserCart;

namespace UserService.UnitTests.Carts.Fakers.Queries;

public class GetUserCartQueryFaker : Faker<GetUserCartQuery>
{
    public GetUserCartQueryFaker()
    {
        RuleFor(c => c.UserId, f => f.Lorem.Word());
    }
}
