using Bogus;
using UserService.BLL.Carts.Commands.DeleteItemFromCart;

namespace UserService.UnitTests.Carts.Fakers.Commands;

public class DeleteItemFromCartCommandFaker : Faker<DeleteItemFromCartCommand> 
{
    public DeleteItemFromCartCommandFaker()
    {
        RuleFor(c => c.UserId, f => f.Lorem.Word());
        RuleFor(c => c.CartItemId, f => f.Random.Guid());
    }
}
