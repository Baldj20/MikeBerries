using Mapster;
using NSubstitute;
using ProductService.BLL;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
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
        await _unitOfWork.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task DeleteProductAsync_WhenProductExists_ShouldReturnSuccessResult(ProductModel productModel)
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
    public async Task DeleteProductAsync_WhenProductDoesNotExist_ShouldReturnFailureResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.DeleteProductAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(Result
                .Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_DELETE_NOT_FOUND)));
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProduct(ProductModel productModel)
    {
        //Arrange
        var productEntity = productModel.Adapt<Product>();
        var id = productEntity.Id;
        _unitOfWork.Products.GetByIdAsync(id, default).Returns(productEntity);

        //Act
        var response = await _productService.GetProductByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(productModel);
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_WhenProductDoesNotExist_ShouldReturnFailureResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Products.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.GetProductByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(new Result<ProductModel>(CustomError
            .ResourceNotFound(LoggingConstants<Product>.RESOURCE_NOT_FOUND)));
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProductAsync_WhenProductExists_ShouldReturnSuccessResult(Product product, ProductModel productModel)
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
    public async Task UpdateProductAsync_WhenProductDoesNotExist_ShouldReturnFailureResult(ProductModel productModel)
    {
        //Arrange
        var id = Guid.NewGuid();
        _productRepository.GetByIdAsync(id, default).Returns((Product)null!);

        //Act
        var response = await _productService.UpdateProductAsync(id, productModel, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(Result
            .Failure(CustomError.ResourceNotFound(LoggingConstants<Product>.RESOURCE_TO_UPDATE_NOT_FOUND)));
        await _productRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());      
    }

    [Theory, AutoDataCustom]
    public async Task GetProductsAsync_ShouldReturnPageSizedProductsList(
        PaginationParams paginationParams,
        ProductFilter filter,
        List<Product> pagedProducts)
    {
        //Arrange
        _productRepository.GetPaged(paginationParams, filter)
            .Returns(pagedProducts);
        var pagedProductModels = pagedProducts.Adapt<List<ProductModel>>();

        //Act
        var response = await _productService.GetProductsAsync(paginationParams, filter, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(pagedProductModels);
        _productRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            Arg.Any<ProductFilter>());
    }

    [Theory, AutoDataCustom]
    public async Task GetProductsAsync_WhenProductsNotFound_ShouldReturnFailureResult(
        PaginationParams paginationParams,
        ProductFilter filter)
    {
        //Arrange
        _productRepository.GetPaged(paginationParams, filter)
            .Returns(new List<Product>());

        //Act
        var response = await _productService.GetProductsAsync(paginationParams, filter, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(new Result<List<ProductModel>>(CustomError
                .ResourceNotFound(LoggingConstants<Product>.RESOURCES_FILTERED_NOT_FOUND)));
        _productRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            Arg.Any<ProductFilter>());
    }
}
