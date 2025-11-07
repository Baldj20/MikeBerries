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
    public async Task<Result> Add([FromForm]CreateProductDto dto, CancellationToken token)
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
    public async Task<Result<GetProductDto>> GetById(Guid id, CancellationToken token)
    {
        var product = await productService.GetProductByIdAsync(id, token);

        return product.IsSuccess ? 
            new Result<GetProductDto>(product.Value.Adapt<GetProductDto>()) : 
            new Result<GetProductDto>(product.Error!);
    }

    [HttpGet]
    public Result<List<GetProductDto>> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProductFilter filter, 
        CancellationToken token)
    {
        var products = productService.GetProducts(paginationParams, filter, token);

        return products.IsSuccess ?
            new Result<List<GetProductDto>>(products.Value.Adapt<List<GetProductDto>>()) :
            new Result<List<GetProductDto>>(products.Error!);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<Result> Update(Guid id, [FromForm]UpdateProductDto dto, CancellationToken token)
    {
        var response = await productService.UpdateProductAsync(id, dto.Adapt<ProductModel>(), token);

        return response;
    }
}
