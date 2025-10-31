namespace ProductService.DAL.Entities;

public class Product : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public Guid ProviderId { get; set; }
    public List<ProductImage> Images { get; set; }
    public Provider Provider { get; set; }
}
