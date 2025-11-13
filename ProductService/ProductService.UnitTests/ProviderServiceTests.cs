using Mapster;
using NSubstitute;
using ProductService.BLL;
using ProductService.BLL.Constants.Logging;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using Shouldly;
using UnitTests;

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
        await _unitOfWork.Received(1).SaveChangesAsync(
            default);
    }

    [Theory, AutoDataCustom]
    public async Task DeleteProviderAsync_WhenProviderExists_ShouldReturnSuccessResult(ProviderModel providerModel)
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
            default);
        await _providerRepository.Received(1).Delete(
            Arg.Is<Provider>(p => p.Id == id));
    }

    [Fact]
    public async Task DeleteProviderAsync_WhenProviderDoesNotExist_ShouldReturnFailureResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns((Provider)null!);
        var errorMessage = LoggingConstants.RESOURCE_TO_DELETE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

        //Act
        var response = await _providerService.DeleteProviderAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(Result
                .Failure(CustomError.ResourceNotFound(errorMessage)));
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            default);
    }

    [Theory, AutoDataCustom]
    public async Task GetProviderByIdAsync_WhenProviderExists_ShouldReturnProvider(ProviderModel providerModel)
    {
        //Arrange
        var providerEntity = providerModel.Adapt<Provider>();
        var id = providerEntity.Id;
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns(providerEntity);

        //Act
        var response = await _providerService.GetProviderByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(providerModel);
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            default);
    }

    [Fact]
    public async Task GetProviderByIdAsync_WhenProviderDoesNotExist_ShouldReturnFailureResult()
    {
        //Arrange
        var id = Guid.NewGuid();
        _unitOfWork.Providers.GetByIdAsync(id, default).Returns((Provider)null!);
        var errorMessage = LoggingConstants.RESOURCE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);
        errorMessage = errorMessage.Replace("{ResourceId}", id.ToString());

        //Act
        var response = await _providerService.GetProviderByIdAsync(id, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(new Result<ProviderModel>(CustomError
            .ResourceNotFound(errorMessage)));
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            default);
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProviderAsync_WhenProviderExists_ShouldReturnSuccessResult(Provider provider, ProviderModel providerModel)
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
            default);
        await _unitOfWork.Received(1).SaveChangesAsync(
            default);
    }

    [Theory, AutoDataCustom]
    public async Task UpdateProviderAsync_WhenProviderDoesNotExist_ShouldReturnFailureResult(ProviderModel providerModel)
    {
        //Arrange
        var id = Guid.NewGuid();
        _providerRepository.GetByIdAsync(id, default).Returns((Provider)null!);
        var errorMessage = LoggingConstants.RESOURCE_TO_UPDATE_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

        //Act
        var response = await _providerService.UpdateProviderAsync(id, providerModel, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(Result
            .Failure(CustomError.ResourceNotFound(errorMessage)));
        await _providerRepository.Received(1).GetByIdAsync(
            Arg.Any<Guid>(),
            default);
    }

    [Theory, AutoDataCustom]
    public void GetProviders_WhenDataIsValid_ShouldReturnPageSizedProvidersList(
        PaginationParams paginationParams,
        List<Provider> pagedProviders)
    {
        //Arrange
        _providerRepository.GetPaged(paginationParams, null!)
            .Returns(pagedProviders);
        var pagedProviderModels = pagedProviders.Adapt<List<ProviderModel>>();

        //Act
        var response = _providerService.GetProviders(paginationParams, null!, default);

        //Assert
        response.IsSuccess.ShouldBeTrue();
        response.Value.ShouldBeEquivalentTo(pagedProviderModels);
        _providerRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            Arg.Any<ProviderFilter>());
    }

    [Theory, AutoDataCustom]
    public void GetProviders_WhenProvidersNotFound_ShouldReturnFailureResult(
        PaginationParams paginationParams,
        ProviderFilter filter)
    {
        //Arrange
        _providerRepository.GetPaged(paginationParams, filter)
            .Returns(new List<Provider>());
        var errorMessage = LoggingConstants.RESOURCES_FILTERED_NOT_FOUND
                .Replace("{ResourceName}", typeof(Provider).Name);

        //Act
        var response = _providerService.GetProviders(paginationParams, filter, default);

        //Assert
        response.IsSuccess.ShouldBeFalse();
        response.ShouldBeEquivalentTo(
            new Result<List<ProviderModel>>(CustomError
                .ResourceNotFound(errorMessage)));
        _providerRepository.Received(1).GetPaged(
            Arg.Any<PaginationParams>(),
            Arg.Any<ProviderFilter>());
    }
}
