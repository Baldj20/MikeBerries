namespace ProductService.BLL.Models;

public class ProviderModel
{
    public string Email { get; set; }
    public string Name { get; set; }
    public List<ProductModel> Products { get; set; } = new();
}
