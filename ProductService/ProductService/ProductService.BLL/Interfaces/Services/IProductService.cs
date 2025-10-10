using ProductService.BLL.DTO;

namespace ProductService.BLL.Interfaces.Services;

public interface IProductService
{
    public Task AddProductAsync(CreateProductDTO dto, CancellationToken token);
    public Task<Result> DeleteProductAsync(Guid id, CancellationToken token);
    public Task<Result> GetProductByIdAsync(Guid id, CancellationToken token);
    public Task<Result> GetProductsAsync(string? title, string? provider,
        decimal? minPrice, decimal? maxPrice, CancellationToken token);
    public Task<Result> UpdateAsync(UpdateProductDTO dto, CancellationToken token);
}
