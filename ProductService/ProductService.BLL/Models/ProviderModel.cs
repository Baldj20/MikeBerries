namespace ProductService.BLL.Models;

public class ProviderModel
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public List<ProductModel> Products { get; set; } = new();
}
