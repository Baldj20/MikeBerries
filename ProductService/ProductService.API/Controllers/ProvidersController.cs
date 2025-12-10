using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Filters.Attributes;
using ProductService.BLL;
using ProductService.BLL.Constants;
using ProductService.BLL.DTO;
using ProductService.BLL.Interfaces.Services;
using ProductService.BLL.Models;
using ProductService.DAL;
using ProductService.DAL.Filters;

namespace ProductService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AddStatusCode]
[ExceptionFilter]
public class ProvidersController(IProviderService providerService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<Result> Add(ProviderDto dto, CancellationToken token)
    {
        var response = await providerService.AddProviderAsync(dto.Adapt<ProviderModel>(), token);

        return response;
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PoliciesNames.ADMIN)]
    public async Task<Result> Delete(Guid id, CancellationToken token)
    {
        var response = await providerService.DeleteProviderAsync(id, User, token);

        return response;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<Result<ProviderDto>> GetById(Guid id, CancellationToken token)
    {
        var provider = await providerService.GetProviderByIdAsync(id, token);

        return provider.IsSuccess ? 
            new Result<ProviderDto>(provider.Value.Adapt<ProviderDto>(), provider.StatusCode) :
            new Result<ProviderDto>(provider.Error!, provider.StatusCode);
    }

    [HttpGet]
    [Authorize]
    public Result<PagedResult<ProviderDto>> GetAllPaged(
        [FromQuery] PaginationParams paginationParams,
        [FromQuery] ProviderFilter filter,
        CancellationToken token)
    {
        var providers = providerService.GetProviders(paginationParams, filter, token);

        return providers.IsSuccess ? 
            new Result<PagedResult<ProviderDto>>(providers.Value.Adapt<PagedResult<ProviderDto>>(), providers.StatusCode) : 
            new Result<PagedResult<ProviderDto>>(providers.Error!, providers.StatusCode);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PoliciesNames.ADMIN)]
    public async Task<Result> Update(Guid id, ProviderDto dto, CancellationToken token)
    {
        var response = await providerService.UpdateProviderAsync(id, User, dto.Adapt<ProviderModel>(), token);

        return response;
    }
}
