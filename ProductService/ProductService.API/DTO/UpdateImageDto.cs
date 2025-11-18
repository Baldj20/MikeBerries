using ProductService.BLL.Models;

namespace ProductService.API.DTO;

public class UpdateImageDto
{
    public IFormFile? Image {  get; set; }
    public string? Url { get; set; }
    public UpdateImageAction Action { get; set; }
}
