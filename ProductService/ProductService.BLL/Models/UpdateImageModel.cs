using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.Models;

public class UpdateImageModel
{
    public IFormFile? Image { get; set; }
    public string? Url { get; set; }
    public UpdateImageAction Action { get; set; }
}
