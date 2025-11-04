using Mapster;
using Newtonsoft.Json;
using ProductService.BLL;
using ProductService.BLL.DTO;
using Shouldly;
using System.Net;

namespace ProductService.IntegrationTests;

public class ProductControllerTests(ProductServiceWebApplicationFactory factory) 
    : BaseTest(factory), IClassFixture<ProductServiceWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task PostProduct_WhenDataIsValid_ShouldReturnSuccess()
    {
        //Arrange
        var formData = TestDataHelper.CreateProductDto();

        // Act
        var response = await _httpClient.PostAsync("api/products", formData);

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task PostProduct_WhenPriceIsNotValid_ShouldReturnFailure(decimal price)
    {
        //Arrange
        var formData = TestDataHelper.CreateProductDto(price: price);

        // Act
        var response = await _httpClient.PostAsync("api/products", formData);

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ShouldReturnSuccess()
    {
        //Arrange
        var product = await Add(TestDataHelper.CreateProductEntity());

        // Act
        var response = await _httpClient.DeleteAsync($"api/products/{product.Id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductNotExists_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"api/products/{id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var serializedContent = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonConvert.DeserializeObject<Result>(serializedContent);

        deserializedResult.ShouldNotBeNull();
        deserializedResult.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ShouldReturnProduct()
    {
        //Arrange
        var product = await Add(TestDataHelper.CreateProductEntity());

        // Act
        var response = await _httpClient.GetAsync($"api/products/{product.Id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var serializedContent = await response.Content.ReadAsStringAsync();
        var productDto = JsonConvert.DeserializeObject<Result<GetProductDto>>(serializedContent);

        productDto.ShouldNotBeNull();
        productDto.Value.ShouldNotBeNull();
        productDto.Value.ShouldBeEquivalentTo(product.Adapt<GetProductDto>());
    }
}
