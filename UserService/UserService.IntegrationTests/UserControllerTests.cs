using System.Net;
using System.Text;
using Newtonsoft.Json;
using Shouldly;

namespace UserService.IntegrationTests;

public class UserControllerTests(UserServiceWebApplicationFactory factory) : BaseTest(factory), IClassFixture<UserServiceWebApplicationFactory>
{
    [Fact]
    public async Task GetUserById_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        //Act
        var response = await Client.GetAsync($"api/Users/{user.Auth0Id}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteUserIfUserExists_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        //Act
        var response = await Client.DeleteAsync($"api/Users/{user.Auth0Id}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteUserIfUserNotExists_ShouldReturnNotFoundCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        //Act
        var response = await Client.DeleteAsync($"api/Users/{id}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UpdateUserIfUserExists_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        var updateDto = UpdateUserDtoFaker.Generate();
        var json = JsonConvert.SerializeObject(updateDto);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        //Act
        var response = await Client.PutAsync($"api/Users/{user.Auth0Id}", stringContent);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task UpdateUserIfUserNotExists_ShouldReturnNotFoundCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var updateDto = UpdateUserDtoFaker.Generate();
        var json = JsonConvert.SerializeObject(updateDto);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        //Act
        var response = await Client.PutAsync($"api/Users/{id}", stringContent);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
    }
}
