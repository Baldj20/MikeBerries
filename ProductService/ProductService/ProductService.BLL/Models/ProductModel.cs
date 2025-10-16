namespace ProductService.BLL.Models;

public class ProductModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<ProductImageModel> Images { get; set; }
    public ProviderModel Provider { get; set; }
}
