using Mapster;
using Microsoft.AspNetCore.Mvc;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvidersController(IProviderService providerService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Add(ProviderDTO dto, CancellationToken token)
    {
        var response = await providerService.AddProviderAsync(dto.Adapt<ProviderModel>(), token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken token)
    {
        var response = await providerService.DeleteProviderAsync(id, token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken token)
    {
        var response = await providerService.GetProviderByIdAsync(id, token);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProviderFilter filter,
        CancellationToken token)
    {
        var response = await providerService.GetProvidersAsync(paginationParams, filter, token);

        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, ProviderDTO dto, CancellationToken token)
    {
        var response = await providerService.UpdateAsync(id, dto.Adapt<ProviderModel>(), token);

        return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
    }
}
