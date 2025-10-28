using AutoFixture;
using Mapster;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ProductService.BLL;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Filters;
using ProductService.UnitTests;
using Xunit;

namespace UnitTests;

public class ProductServiceTests : Mocks
{
    [Theory, AutoDataCustom]
    public async Task AddProductAsync_WhenDataIsValid_ShouldReturnSuccessResult(ProductModel productModel)
    {
        //Arrange
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

    [Theory, AutoDataCustom]
    public async Task DeleteProductAsync_WhenProductExists(ProductModel productModel)
    {
        //Arrange
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
    public async Task DeleteProductAsync_WhenProductDoesNotExist()
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

    [Theory, AutoDataCustom]
    public async Task GetProductByIdAsync_WhenProductExists(ProductModel productModel)
    {
        //Arrange
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
    public async Task GetProductByIdAsync_WhenProductDoesNotExist()
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

    [Theory, AutoDataCustom]
    public async Task UpdateProductAsync_WhenProductExists(Product product, ProductModel productModel)
    {
        //Arrange
        var id = product.Id;
        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(product);

        //Act
        await _productService.UpdateProductAsync(id, productModel, default);

        //Assert
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());

        Assert.Equal(product.Title, productModel.Title);
        Assert.Equal(product.Description, productModel.Description);
        Assert.Equal(product.Price, productModel.Price);
        Assert.Equal(product.Provider.Name, productModel.Provider.Name);
        Assert.Equal(product.Provider.Email, productModel.Provider.Email);
        Assert.Equal(product.Images.Count, productModel.Images.Count);
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProductAsync_WhenProductDoesNotExist(ProductModel productModel)
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var result = await _productService.UpdateProductAsync(id, productModel, default);

        //Assert
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        Assert.Equal(result,
            Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_UPDATE_NOT_FOUND)));
    }

    [Theory, AutoDataCustom]
    public async Task GetProductsAsync_ShouldReturnPageSizedProductsList(
        PaginationParams paginationParams,
        List<Product> pagedProducts)
    {
        //Arrange       
        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Products.GetPaged(paginationParams, null!)
            .Returns(pagedProducts.AsQueryable());       

        //Act
        await _productService.GetProductsAsync(paginationParams, null!, default);

        //Assert
        _productRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(), 
            null!);
    }
}
