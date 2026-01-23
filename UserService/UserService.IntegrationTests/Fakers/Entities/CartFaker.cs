using Bogus;
using UserService.DAL.Entities;

namespace UserService.IntegrationTests.Fakers.Entities;

public class CartFaker : Faker<Cart>
{
    public CartFaker()
    {
        RuleFor(c => c.UserId, f => f.Lorem.Word());
        RuleFor(c => c.Items, new CartItemFaker().Generate(10));
        RuleFor(c => c.TotalCount, f => f.Random.Int(1, 50));
        RuleFor(c => c.TotalPrice, f => f.Random.Int(1000,2000));
    }
}
