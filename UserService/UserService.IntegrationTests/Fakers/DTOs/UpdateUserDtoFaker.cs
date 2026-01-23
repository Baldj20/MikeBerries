using Bogus;
using UserService.API.DTOs;

namespace UserService.IntegrationTests.Fakers.DTOs;

public class UpdateUserDtoFaker : Faker<UpdateUserDto>
{
    public UpdateUserDtoFaker()
    {
        RuleFor(c => c.Email, f => f.Person.Email);
        RuleFor(c => c.Name, f => f.Person.FullName);
    }
}
