using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.DAL.Entities;
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

    public async Task<Result> GetProductsAsync(string? title, string? provider, 
        decimal? minPrice, decimal? maxPrice, CancellationToken token)
    {
        var query = await _unitOfWork.Products.GetAllAsync(token);

        if (title is not null) query = query.Where(p => p.Title == title);

        if (provider is not null) query = query.Where(p => p.Provider.Name == provider);

        if (minPrice is not null) query = query.Where(p => p.Price >= minPrice);

        if (maxPrice is not null) query = query.Where(p => p.Price <= maxPrice);

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

        await _unitOfWork.Products.Update(dto.Adapt<Product>());

        await _unitOfWork.SaveChangesAsync(token);

        return Result.Success();
    }
}
