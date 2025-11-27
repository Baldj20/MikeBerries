using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.Models;

public class ProductImageModel
{
    public string? Url { get; set; }
    public IFormFile? Image { get; set; }
    public required ProductModel Product { get; set; }
}
