using AutoFixture;
using Mapster;
using NSubstitute;
using ProductService.BLL.Models;
using ProductService.DAL.Entities;
using Xunit;

namespace UnitTests;

public class ProductServiceTests : Mocks
{
    [Fact]
    public async Task AddProductAsync_WhenDataIsValid_ShouldReturnSuccessResult()
    {
        //Arrange
        var productModel = _fixture.Create<ProductModel>();
        _unitOfWork.Products.Returns(_productRepository);

        //Act
        var response = await _productService.AddProductAsync(productModel, default);

        //Assert
        Assert.True(response.IsSuccess);
        await _productRepository.Received(1).AddAsync(
            Arg.Is<Product>(p => p.Title == productModel.Title && 
                                 p.Description == productModel.Description && 
                                 p.Price == productModel.Price),
default);
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductExists()
    {
        //Arrange
        var productModel = _fixture.Create<ProductModel>();
        var productEntity = productModel.Adapt<Product>();
        var id = productEntity.Id;

        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(productEntity);
        _unitOfWork.Products.Delete(productEntity).Returns(Task.CompletedTask);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        Assert.True(response.IsSuccess);
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _productRepository.Received(1).Delete(
            Arg.Is<Product>(p => p.Id == id));
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductDoesNotExists()
    {
        //Arrange
        var id = Guid.NewGuid();

        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        Assert.False(response.IsSuccess);
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductExists()
    {
        //Arrange
        var productModel = _fixture.Create<ProductModel>();
        var productEntity = productModel.Adapt<Product>();
        var id = productEntity.Id;

        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(productEntity);

        //Act
        var response = await _productService.GetProductByIdAsync(id, default);

        //Assert
        Assert.True(response.IsSuccess);
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExists()
    {
        //Arrange
        var id = Guid.NewGuid();

        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        Assert.False(response.IsSuccess);
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

}
