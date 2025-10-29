using Mapster;
using NSubstitute;
using ProductService.BLL;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.UnitTests;
using Shouldly;
using Xunit;

namespace UnitTests;

public class ProductServiceTests : Mocks
{
    [Theory, AutoDataCustom]
    public async Task AddProductAsync_WhenDataIsValid_ShouldReturnSuccessResult(ProductModel productModel)
    {
        //Act
        var response = await _productService.AddProductAsync(productModel, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
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

        _unitOfWork.Products.GetByIdAsync(id, default).Returns(productEntity);
        _unitOfWork.Products.Delete(productEntity).Returns(Task.CompletedTask);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
        await _productRepository.Received(1).Delete(
            Arg.Is<Product>(p => p.Id == id));
    }

    [Fact]
    public async Task DeleteProductAsync_WhenProductDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
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
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(productEntity);

        //Act
        var response = await _productService.GetProductByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProductAsync_WhenProductExists(Product product, ProductModel productModel)
    {
        //Arrange
        var id = product.Id;
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(product);
        var imageUrls = productModel.Images.Select(i => i.Url).ToList();

        //Act
        var response = await _productService.UpdateProductAsync(id, productModel, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());

        product.Title.ShouldBeEquivalentTo(productModel.Title);
        product.Description.ShouldBeEquivalentTo(productModel.Description);
        product.Price.ShouldBeEquivalentTo(productModel.Price);
        product.Provider.Name.ShouldBeEquivalentTo(productModel.Provider.Name);
        product.Provider.Email.ShouldBeEquivalentTo(productModel.Provider.Email);
        product.Images.Count.ShouldBeEquivalentTo(productModel.Images.Count);

        for (int i = 0; i < imageUrls.Count; i++)
        {
            product.Images[i].Url.ShouldBeEquivalentTo(imageUrls[i]);
        }
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProductAsync_WhenProductDoesNotExist(ProductModel productModel)
    {
        //Arrange
        var id = Guid.NewGuid();
        _productRepository.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.UpdateProductAsync(id, productModel, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        response.ShouldBeEquivalentTo(
            Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_UPDATE_NOT_FOUND)));
    }

    [Theory, AutoDataCustom]
    public async Task GetProductsAsync_ShouldReturnPageSizedProductsList(
        PaginationParams paginationParams,
        List<Product> pagedProducts)
    {
        //Arrange
        _productRepository.GetPaged(paginationParams, null!)
            .Returns(pagedProducts);
        var pagedProductModels = pagedProducts.Adapt<List<ProductModel>>();

        //Act
        var response = await _productService.GetProductsAsync(paginationParams, null!, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(pagedProductModels);
        _productRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            null!);
    }
}
