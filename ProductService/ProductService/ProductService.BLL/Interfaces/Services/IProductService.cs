using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.BLL.Interfaces.Services;

public interface IProductService
{
    Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token);
    Task<Result> DeleteProductAsync(Guid id, CancellationToken token);
    Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token);
    Task<Result<List<ProductModel>>> GetProductsAsync(PaginationParams paginationParams,
        ProductFilter filter, CancellationToken token);
    Task<Result> UpdateAsync(Guid id, ProductModel productModel, CancellationToken token);
}
