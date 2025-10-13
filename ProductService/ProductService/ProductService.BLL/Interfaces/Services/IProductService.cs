using ProductService.BLL.DTO;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.BLL.Interfaces.Services;

public interface IProductService
{
    public Task<Result> AddProductAsync(CreateProductDTO dto, CancellationToken token);
    public Task<Result> DeleteProductAsync(Guid id, CancellationToken token);
    public Task<Result<GetProductDTO>> GetProductByIdAsync(Guid id, CancellationToken token);
    public Task<Result<List<GetProductDTO>>> GetProductsAsync(PaginationParams paginationParams,
        ProductFilter filter, CancellationToken token);
    public Task<Result> UpdateAsync(UpdateProductDTO dto, CancellationToken token);
}
