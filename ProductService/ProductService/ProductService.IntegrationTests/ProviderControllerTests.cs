using Mapster;
using ProductService.BLL.DTO;
using Shouldly;
using System.Net;

namespace ProductService.IntegrationTests;

public class ProviderControllerTests(ProductServiceWebApplicationFactory factory)
    : BaseTest(factory), IClassFixture<ProductServiceWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task PostProvider_WhenDataIsValid_ShouldReturnSuccess()
    {
        //Arrange
        var content = TestDataHelper.CreateProviderStringContent();

        // Act
        var response = await _httpClient.PostAsync("api/providers", content);

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }

    [Fact]  
    public async Task PostProvider_WhenEmailIsNotValid_ShouldReturnFailure()
    {
        //Arrange
        var content = TestDataHelper.CreateProviderStringContent(providerEmail: "sometext");

        // Act
        var response = await _httpClient.PostAsync("api/providers", content);

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteProvider_WhenProviderExists_ShouldReturnSuccess()
    {
        //Arrange
        var provider = await Add(TestDataHelper.CreateProviderEntity());

        // Act
        var response = await _httpClient.DeleteAsync($"api/providers/{provider.Id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteProvider_WhenProviderNotExists_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.DeleteAsync($"api/providers/{id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task GetProviderById_WhenProviderExists_ShouldReturnProvider()
    {
        //Arrange
        var provider = await Add(TestDataHelper.CreateProviderEntity());

        // Act
        var response = await _httpClient.GetAsync($"api/providers/{provider.Id}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var productDto = await DeserializeResponseTo<ProviderDto>(response);

        productDto.ShouldNotBeNull();
        productDto.Value.ShouldNotBeNull();
        productDto.Value.ShouldBeEquivalentTo(provider.Adapt<ProviderDto>());
    }

    [Fact]
    public async Task GetProviderById_WhenProviderNotExists_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"api/providers/{id}");

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
    public async Task GetProvidersPaged_WhenProvidersExist_ShouldReturnProvidersPagedList(
        int page,
        int pageSize)
    {
        //Arrange
        var providers = new[]
        {
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity()
        };

        await AddRange(providers);

        // Act
        var response = await _httpClient.GetAsync($"api/providers?page={page}&pagesize={pageSize}");

        // Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponseTo<List<ProviderDto>>(response);

        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBeEquivalentTo(pageSize);
    }

    [Theory]
    [InlineData(1, 2, "chel")]
    [InlineData(2, 2, "chel")]
    [InlineData(1, 3, "chel")]
    public async Task GetProvidersPaged_WhenProvidersWithNameExist_ShouldReturnProvidersPagedList(
        int page,
        int pageSize,
        string name)
    {
        //Arrange
        var products = new[]
        {
            TestDataHelper.CreateProviderEntity(providerName: "chel"),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(providerName: "chel"),
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(providerName: "chel"),
            TestDataHelper.CreateProviderEntity(providerName: "chel")
        };

        await AddRange(products);

        // Act
        var response = await _httpClient.GetAsync($"api/providers?page={page}&pagesize={pageSize}&name={name}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponseTo<List<ProviderDto>>(response);

        result.ShouldNotBeNull();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBeEquivalentTo(pageSize);
        for (int i = 0; i < pageSize; i++)
        { 
            result.Value[i].Name.ShouldBeEquivalentTo(name);
        }
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    public async Task GetProvidersPaged_WhenPageIsEmpty_ShouldReturnFailure(
        int page,
        int pageSize)
    {
        //Arrange
        var products = new[]
        {
            TestDataHelper.CreateProviderEntity(),
            TestDataHelper.CreateProviderEntity(),
        };

        await AddRange(products);

        // Act
        var response = await _httpClient.GetAsync($"api/providers?page={page}&pagesize={pageSize}");

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateProvider_WhenProviderExist_ShouldUpdateProvider()
    {
        //Arrange
        var provider = await Add(TestDataHelper.CreateProviderEntity());
        var content = TestDataHelper.CreateProviderStringContent();

        //Act
        var response = await _httpClient.PutAsync($"api/providers/{provider.Id}", content);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task UpdateProvider_WhenProviderNotExist_ShouldReturnFailure()
    {
        //Arrange
        var id = Guid.NewGuid();
        var content = TestDataHelper.CreateProviderStringContent();

        //Act
        var response = await _httpClient.PutAsync($"api/providers/{id}", content);

        //Assert
        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

        var result = await DeserializeResponse(response);
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
    }
}
