using Medallion.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Polly;
using Polly.Registry;
using ProductService.API.Constants;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Services;
using ProductService.DAL.Interfaces.Repositories;
using System.Security.Claims;

namespace UnitTests;

public class Mocks
{
    protected readonly ILogger<ProductService.BLL.Services.ProductService> _productServiceLogger;
    protected readonly ILogger<ProviderService> _providerServiceLogger;
    protected readonly IProductRepository _productRepository;
    protected readonly IProviderRepository _providerRepository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IProductService _productService;
    protected readonly IProviderService _providerService;
    protected readonly IFileRepository _fileRepository;
    protected readonly ICacheRepository _cacheRepository;
    protected readonly IAuthorizationService _authService;
    protected readonly ResiliencePipelineProvider<string> _pipelineProvider;
    protected readonly IDistributedLockProvider _lockProvider;
    protected readonly ClaimsPrincipal user;

    protected Mocks()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _providerRepository = Substitute.For<IProviderRepository>();
        _fileRepository = Substitute.For<IFileRepository>();
        _cacheRepository = Substitute.For<ICacheRepository>();
        _authService = Substitute.For<IAuthorizationService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Providers.Returns(_providerRepository);
        _unitOfWork.Files.Returns(_fileRepository);
        _pipelineProvider = Substitute.For<ResiliencePipelineProvider<string>>();
        _pipelineProvider.GetPipeline(Arg.Any<string>())
            .Returns(ResiliencePipeline.Empty);
        _productServiceLogger = Substitute.For<ILogger<ProductService.BLL.Services.ProductService>>();
        _providerServiceLogger = Substitute.For<ILogger<ProviderService>>();

        _lockProvider = Substitute.For<IDistributedLockProvider>();

        _productService = new ProductService.BLL.Services.ProductService(_unitOfWork, _cacheRepository, 
            _authService, _productServiceLogger, _lockProvider, _pipelineProvider);
        _providerService = new ProviderService(_unitOfWork, _cacheRepository,
            _providerServiceLogger, _lockProvider, _pipelineProvider);

        user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, string.Empty),
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, RolesNames.ADMIN)
        }, "TestAuth"));

        _authService
            .AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<object>(), Arg.Any<string>())
            .Returns(AuthorizationResult.Success());
    }
}
