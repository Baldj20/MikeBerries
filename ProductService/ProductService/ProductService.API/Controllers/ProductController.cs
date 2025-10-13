using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.API.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Add(CreateProductDTO dto, CancellationToken token)
    {
        var response = await productService.AddProductAsync(dto.Adapt<ProductModel>(), token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken token)
    {
        var response = await productService.DeleteProductAsync(id, token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken token)
    {
        var response = await productService.GetProductByIdAsync(id, token);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProductFilter filter, 
        CancellationToken token)
    {
        var response = await productService.GetProductsAsync(paginationParams, filter, token);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateProductDTO dto, CancellationToken token)
    {
        var response = await productService.UpdateAsync(id, dto.Adapt<ProductModel>(), token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }
}
