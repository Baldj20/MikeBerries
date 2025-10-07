namespace ProductService.DAL.Entities;

public class Product
{
    public string Article { get; set; }
    public string Description { get; set; }
    public string ProviderEmail { get; set; }
    public int Price { get; set; }
    public ICollection<ProductImage> Images { get; set; }
    public Provider Provider { get; set; }
}
