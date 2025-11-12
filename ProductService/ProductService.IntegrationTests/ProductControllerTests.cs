using Mapster;
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
        var formData = TestDataHelper.CreateProductDtoFormData();

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
        var formData = TestDataHelper.CreateProductDtoFormData(price: price);

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

        var result = await DeserializeResponse(response);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
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

        var productDto = await DeserializeResponseTo<GetProductDto>(response);

        productDto.ShouldNotBeNull();
        productDto.Value.ShouldNotBeNull();
        productDto.Value.ShouldBeEquivalentTo(product.Adapt<GetProductDto>());
    }

    [Fact]
    public async Task GetProductById_WhenProductNotExists_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"api/products/{id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 2)]
    public async Task GetProductsPaged_WhenProductsExist_ShouldReturnProductsPagedList(
        int page,
        int pageSize)
    {
        //Arrange
        var products = new[]
        {
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity()
        };

        await AddRange(products);

        // Act
        var response = await _httpClient.GetAsync($"api/products?page={page}&pagesize={pageSize}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponseTo<List<GetProductDto>>(response);

        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBeEquivalentTo(pageSize);
    }

    [Theory]
    [InlineData(1, 2, "flugegeheimen")]
    [InlineData(2, 2, "flugegeheimen")]
    [InlineData(1, 3, "flugegeheimen")]
    public async Task GetProductsPaged_WhenProductsWithTitleExist_ShouldReturnProductsPagedList(
        int page,
        int pageSize,
        string title)
    {
        //Arrange
        var products = new[]
        {
            TestDataHelper.CreateProductEntity(title: "flugegeheimen"),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(title: "flugegeheimen"),
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(title: "flugegeheimen"),
            TestDataHelper.CreateProductEntity(title: "flugegeheimen")
        };

        await AddRange(products);

        // Act
        var response = await _httpClient.GetAsync($"api/products?page={page}&pagesize={pageSize}&title={title}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponseTo<List<GetProductDto>>(response);

        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBeEquivalentTo(pageSize);
        for (int i = 0; i < pageSize; i++)
        {
            result.Value[i].Title.ShouldBeEquivalentTo(title);
        }
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    public async Task GetProductsPaged_WhenPageIsEmpty_ShouldReturnFailure(
        int page,
        int pageSize)
    {
        //Arrange
        var products = new[]
        {
            TestDataHelper.CreateProductEntity(),
            TestDataHelper.CreateProductEntity(),
        };

        await AddRange(products);

        // Act
        var response = await _httpClient.GetAsync($"api/products?page={page}&pagesize={pageSize}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateProduct_WhenProductExist_ShouldUpdateProduct()
    {
        //Arrange
        var product = await Add(TestDataHelper.CreateProductEntity());
        var updateFormData = TestDataHelper.UpdateProductDtoFormData();

        //Act
        var response = await _httpClient.PutAsync($"api/products/{product.Id}", updateFormData);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateProduct_WhenProductNotExist_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();
        var updateFormData = TestDataHelper.UpdateProductDtoFormData();

        //Act
        var response = await _httpClient.PutAsync($"api/products/{id}", updateFormData);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }
}
