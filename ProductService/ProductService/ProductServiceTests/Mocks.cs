using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Services;
using ProductService.DAL.Interfaces.Repositories;
using ProductService.DAL.Repositories;

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
    
    protected Mocks()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _providerRepository = Substitute.For<IProviderRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _unitOfWork.Products.Returns(_productRepository);
        _unitOfWork.Providers.Returns(_providerRepository);
        _productServiceLogger = Substitute.For<ILogger<ProductService.BLL.Services.ProductService>>();
        _providerServiceLogger = Substitute.For<ILogger<ProviderService>>();
        _productService = new ProductService.BLL.Services.ProductService(_unitOfWork, _productServiceLogger);
        _providerService = new ProviderService(_unitOfWork, _providerServiceLogger);
    }
}
