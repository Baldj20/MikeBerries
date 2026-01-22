using Bogus;
using UserService.BLL.Carts.Commands.AddItemToCart;

namespace UserService.UnitTests.Carts.Fakers.Commands;

public class AddItemToCartCommandFaker : Faker<AddItemToCartCommand>
{
    public AddItemToCartCommandFaker()
    {
        RuleFor(c => c.UserId, f => f.Lorem.Word());
        RuleFor(c => c.ProductId, f => f.Random.Guid());
        RuleFor(c => c.Count, f => f.Random.Int(1, 50));
    }
}
