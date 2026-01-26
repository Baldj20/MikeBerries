using System.Net;
using System.Text;
using Newtonsoft.Json;
using Shouldly;
using UserService.DAL.Entities;

namespace UserService.IntegrationTests;

public class CartControllerTests(UserServiceWebApplicationFactory factory) : BaseTest(factory), IClassFixture<UserServiceWebApplicationFactory>
{
    [Fact]
    public async Task GetUserCart_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        //Act
        var response = await Client.GetAsync($"api/Carts/{user.Auth0Id}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task AddItemToCart_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        var item = AddItemDtoFaker.Generate();
        var json = JsonConvert.SerializeObject(item);
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        
        //Act
        var response = await Client.PostAsync($"api/Carts/{user.Auth0Id}/Items", stringContent);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteItemFromCartIfItemExists_ShouldReturnSuccessCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        await AddUserToDatabaseAsync(user);
        
        var itemId = user.Cart.Items.First().Id;
        
        //Act
        var response = await Client.DeleteAsync($"api/Carts/{user.Auth0Id}/Items/{itemId}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteItemFromCartIfItemNotExists_ShouldReturnNotFoundCode()
    {
        // Arrange
        var user = UserFaker.Generate();
        user.Cart.Items = new List<CartItem>();
        await AddUserToDatabaseAsync(user);
        
        var itemId = Guid.NewGuid();
        
        //Act
        var response = await Client.DeleteAsync($"api/Carts/{user.Auth0Id}/Items/{itemId}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
    }
}
