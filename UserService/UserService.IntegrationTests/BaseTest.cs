using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;
using UserService.DAL.Entities;
using UserService.IntegrationTests.Fakers.DTOs;
using UserService.IntegrationTests.Fakers.Entities;

namespace UserService.IntegrationTests;

public class BaseTest(UserServiceWebApplicationFactory factory)
{
    protected readonly UserServiceWebApplicationFactory Factory = factory;
    protected readonly HttpClient Client = factory.CreateClient();
    
    protected readonly AddItemDtoFaker AddItemDtoFaker = new AddItemDtoFaker();
    
    protected readonly CartItemFaker CartItemFaker = new();
    protected readonly CartFaker CartFaker = new();
    protected readonly UserFaker UserFaker = new();
    protected async Task AddUserToDatabaseAsync(User user)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();
        
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}
