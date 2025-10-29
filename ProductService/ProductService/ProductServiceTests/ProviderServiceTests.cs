using Mapster;
using NSubstitute;
using ProductService.BLL;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using Shouldly;
using UnitTests;
using Xunit;

namespace ProductService.UnitTests;

public class ProviderServiceTests : Mocks
{
    [Theory, AutoDataCustom]
    public async Task AddProviderAsync_WhenDataIsValid_ShouldReturnSuccessResult(ProviderModel providerModel)
    {    
        //Act
        var response = await _providerService.AddProviderAsync(providerModel, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _providerRepository.Received(1).AddAsync(
            Arg.Is<Provider>(p => p.Name == providerModel.Name &&
                                 p.Email == providerModel.Email),
            default);
    }

    [Theory, AutoDataCustom]
    public async Task DeleteProviderAsync_WhenProviderExists(ProviderModel providerModel)
    {
        //Arrange
        var providerEntity = providerModel.Adapt<Provider>();
        var id = providerEntity.Id;
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns(providerEntity);
        _unitOfWork.Providers.Delete(providerEntity).Returns(Task.CompletedTask);

        //Act
        var response = await _providerService.DeleteProviderAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _providerRepository.Received(1).Delete(
            Arg.Is<Provider>(p => p.Id == id));
    }

    [Fact]
    public async Task DeleteProviderAsync_WhenProviderDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns((Provider)null!);

        //Act
        var response = await _providerService.DeleteProviderAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task GetProviderByIdAsync_WhenProviderExists(ProviderModel providerModel)
    {
        //Arrange
        var providerEntity = providerModel.Adapt<Provider>();
        var id = providerEntity.Id;
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns(providerEntity);

        //Act
        var response = await _providerService.GetProviderByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProviderByIdAsync_WhenProviderDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns((Provider)null!);

        //Act
        var response = await _providerService.DeleteProviderAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProviderAsync_WhenProviderExists(Provider provider, ProviderModel providerModel)
    {
        //Arrange
        var id = provider.Id;
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns(provider);

        //Act
        var response = await _providerService.UpdateProviderAsync(id, providerModel, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(
            Arg.Any<CancellationToken>());
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProviderAsync_WhenProviderDoesNotExist(ProviderModel providerModel)
    {
        //Arrange
        var id = Guid.NewGuid();
        _providerRepository.GetByIdAsync(id, default).Returns((Provider)null!);

        //Act
        var response = await _providerService.UpdateProviderAsync(id, providerModel, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            Arg.Any<CancellationToken>());
        response.ShouldBeEquivalentTo(
            Result.Failure(CustomError.ResourceNotFound(LoggingConstants<Provider>.RESOURCE_TO_UPDATE_NOT_FOUND)));
    }

    [Theory, AutoDataCustom]
    public async Task GetProvidersAsync_ShouldReturnPageSizedProvidersList(
        PaginationParams paginationParams,
        List<Provider> pagedProviders)
    {
        //Arrange
        _providerRepository.GetPaged(paginationParams, null!)
            .Returns(pagedProviders);
        var pagedProviderModels = pagedProviders.Adapt<List<ProviderModel>>();

        //Act
        var response = await _providerService.GetProvidersAsync(paginationParams, null!, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(pagedProviderModels);
        _providerRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            null!);
    }
}
