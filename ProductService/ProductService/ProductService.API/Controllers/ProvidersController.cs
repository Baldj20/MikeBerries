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
public class ProvidersController(IProviderService providerService) : ControllerBase
{
    [HttpPost]
    public async Task<Result> Add(ProviderDto dto, CancellationToken token)
    {
        var response = await providerService.AddProviderAsync(dto.Adapt<ProviderModel>(), token);

        return response;
    }

    [HttpDelete("{id}")]
    public async Task<Result> Delete(Guid id, CancellationToken token)
    {
        var response = await providerService.DeleteProviderAsync(id, token);

        return response;
    }

    [HttpGet("{id}")]
    public async Task<Result<ProviderDto>> GetById(Guid id, CancellationToken token)
    {
        var provider = await providerService.GetProviderByIdAsync(id, token);

        return provider.IsSuccess ? 
            new Result<ProviderDto>(provider.Value.Adapt<ProviderDto>()) :
            new Result<ProviderDto>(provider.Error!);
    }

    [HttpGet]
    public Result<List<ProviderDto>> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProviderFilter filter,
        CancellationToken token)
    {
        var providers = providerService.GetProviders(paginationParams, filter, token);

        return providers.IsSuccess ? 
            new Result<List<ProviderDto>>(providers.Value.Adapt<List<ProviderDto>>()) : 
            new Result<List<ProviderDto>>(providers.Error!);
    }

    [HttpPut("{id}")]
    public async Task<Result> Update(Guid id, ProviderDto dto, CancellationToken token)
    {
        var response = await providerService.UpdateProviderAsync(id, dto.Adapt<ProviderModel>(), token);

        return response;
    }
}
