using MessagePack;
using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.Models;

[MessagePackObject(keyAsPropertyName: true)]
public class ProductImageModel
{
    public string? Url { get; set; }

    [IgnoreMember]
    public IFormFile? Image { get; set; }
    public required ProductModel Product { get; set; }
}
