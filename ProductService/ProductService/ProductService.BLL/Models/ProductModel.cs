namespace ProductService.BLL.Models;

public class ProductModel
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public required List<ProductImageModel> Images { get; set; } = new();
    public required ProviderModel Provider { get; set; }
}
