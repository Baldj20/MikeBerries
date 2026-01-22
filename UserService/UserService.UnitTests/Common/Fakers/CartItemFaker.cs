using Bogus;
using UserService.DAL.Entities;

namespace UserService.UnitTests.Common.Fakers;

public class CartItemFaker : Faker<CartItem>
{
    public CartItemFaker()
    {
        RuleFor(c => c.Id, f => f.Random.Guid());
        RuleFor(c => c.ProductId, f => f.Random.Guid());
        RuleFor(c => c.Count, f => f.Random.Int(1, 50));
        RuleFor(c => c.UserId, f => f.Lorem.Word());
        RuleFor(c => c.IsChosen, f => f.Random.Bool());
    }
}
