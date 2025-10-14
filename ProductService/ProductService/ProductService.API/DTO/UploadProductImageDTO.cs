using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.DTO;

public class UploadProductImageDTO
{
    public IFormFile Image { get; }
}
