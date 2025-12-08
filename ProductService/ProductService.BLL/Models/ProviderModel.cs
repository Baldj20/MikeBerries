using MessagePack;

namespace ProductService.BLL.Models;

[MessagePackObject(keyAsPropertyName: true)]
public class ProviderModel
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public List<ProductModel> Products { get; set; } = new();
}
