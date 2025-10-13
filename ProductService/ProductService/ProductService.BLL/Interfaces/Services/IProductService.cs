using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.BLL.Interfaces.Services;

public interface IProductService
{
    public Task<Result> AddProductAsync(ProductModel productModel, CancellationToken token);
    public Task<Result> DeleteProductAsync(Guid id, CancellationToken token);
    public Task<Result<ProductModel>> GetProductByIdAsync(Guid id, CancellationToken token);
    public Task<Result<List<ProductModel>>> GetProductsAsync(PaginationParams paginationParams,
        ProductFilter filter, CancellationToken token);
    public Task<Result> UpdateAsync(Guid id, ProductModel productModel, CancellationToken token);
}
