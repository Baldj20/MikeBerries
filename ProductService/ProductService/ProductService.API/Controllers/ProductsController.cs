using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProductService.BLL;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<Result> Add([FromForm]CreateProductDTO dto, CancellationToken token)
    {
        var response = await productService.AddProductAsync(dto.Adapt<ProductModel>(), token);

        return response;
    }

    [HttpDelete("{id}")]
    public async Task<Result> Delete(Guid id, CancellationToken token)
    {
        var response = await productService.DeleteProductAsync(id, token);

        return response;
    }

    [HttpGet("{id}")]
    public async Task<Result<GetProductDTO>> GetById(Guid id, CancellationToken token)
    {
        var product = await productService.GetProductByIdAsync(id, token);

        return product.IsSuccess ? 
            new Result<GetProductDTO>(product.Value.Adapt<GetProductDTO>()) : 
            new Result<GetProductDTO>(product.Error!);
    }

    [HttpGet]
    public Result<List<GetProductDTO>> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProductFilter filter, 
        CancellationToken token)
    {
        var products = productService.GetProducts(paginationParams, filter, token);

        return products.IsSuccess ?
            new Result<List<GetProductDTO>>(products.Value.Adapt<List<GetProductDTO>>()) :
            new Result<List<GetProductDTO>>(products.Error!);
    }

    [HttpPut("{id}")]
    public async Task<Result> Update(Guid id, [FromForm]UpdateProductDTO dto, CancellationToken token)
    {
        var response = await productService.UpdateProductAsync(id, dto.Adapt<ProductModel>(), token);

        return response;
    }
}
