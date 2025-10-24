using AutoFixture;
using ProductService.BLL.Models;
using Xunit;

namespace UnitTests;

public class ProductServiceTests : Mocks
{
    [Fact]
    public async Task AddProductAsync_WhenDataIsValid_ShouldReturnSuccessResult()
    {
        //Arrange
        var productModel = _fixture.Create<ProductModel>();

        var response = await _productService.AddProductAsync(productModel, default);

        Assert.True(response.IsSuccess);
    }
}
