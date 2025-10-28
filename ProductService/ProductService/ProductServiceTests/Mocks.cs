using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductService.BLL.Interfaces.Services;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Repositories;

namespace UnitTests;

public class Mocks
{
    protected readonly ILogger<ProductService.BLL.Services.ProductService> _logger;
    protected readonly IRepository<Product> _productRepository;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly MikeBerriesDBContext _dbContext;
    protected readonly IProductService _productService;
    protected Mocks()
    {
        _productRepository = Substitute.For<IRepository<Product>>();
        _logger = Substitute.For<ILogger<ProductService.BLL.Services.ProductService>>();
        //_dbContext = Substitute.For<MikeBerriesDBContext>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _productService = new ProductService.BLL.Services.ProductService(_unitOfWork, _logger);
    }
}
