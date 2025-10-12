using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.DAL;
using ProductService.DAL.Entities;
using ProductService.DAL.Filters;
using ProductService.DAL.Interfaces.Repositories;

namespace ProductService.BLL.Services;

public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    private IUnitOfWork _unitOfWork => unitOfWork;

    public async Task AddProductAsync(CreateProductDTO dto, CancellationToken token)
    {
        var product = dto.Adapt<Product>();

        await _unitOfWork.Products.AddAsync(product, token);

        await _unitOfWork.SaveChangesAsync(token);
    }

    public async Task<Result> DeleteProductAsync(Guid id, CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, token);

        if (product is not null)
        {
            await _unitOfWork.Products.Delete(product);

            return Result.Success();
        }
        else
        {
            return Result
                .Failure(CustomError.ResourceNotFound("resource to delete is not found"));
        }
    }

    public async Task<Result> GetProductByIdAsync(Guid id, CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, token);

        return product is null ? 
            Result.Failure(CustomError.ResourceNotFound("resource with this id does not exist")):
            new Result<GetProductDTO>(product.Adapt<GetProductDTO>());
    }

    public async Task<Result> GetProductsAsync(PaginationParams paginationParams, 
        ProductFilter filter, CancellationToken token)
    {
        var query = _unitOfWork.Products.GetPaged(paginationParams, filter);

        var result = await query.ToListAsync(token); 

        return result.Count == 0?
            Result.Failure(CustomError.ResourceNotFound("resources with these filters do not exist")) :
            new Result<List<GetProductDTO>>(result.Adapt<List<GetProductDTO>>());
    }

    public async Task<Result> UpdateAsync(UpdateProductDTO dto, CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId, token);

        if (product is null) 
            return Result.Failure(CustomError.ResourceNotFound("resource to update does not exist"));

        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Provider = dto.Adapt<Provider>();
        product.Images = dto.Adapt<List<ProductImage>>();
        product.Price = dto.Price;

        await _unitOfWork.SaveChangesAsync(token);

        return Result.Success();
    }
}
