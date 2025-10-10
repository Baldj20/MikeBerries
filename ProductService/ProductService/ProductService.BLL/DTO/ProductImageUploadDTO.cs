using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.DTO;

public class ProductImageUploadDTO
{
    public IFormFile Image { get; }
}
