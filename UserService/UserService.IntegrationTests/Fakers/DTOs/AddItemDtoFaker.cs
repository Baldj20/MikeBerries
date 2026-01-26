using Bogus;
using UserService.API.DTOs;

namespace UserService.IntegrationTests.Fakers.DTOs;

public class AddItemDtoFaker : Faker<AddItemDto>
{
    public AddItemDtoFaker()
    {
        RuleFor(c => c.ProductId, f => f.Random.Guid());
        RuleFor(c => c.Count, f => f.Random.Int(1, 50));
    }
}
