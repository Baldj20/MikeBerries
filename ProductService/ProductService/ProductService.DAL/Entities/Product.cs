namespace ProductService.DAL.Entities;

public class Product : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid ProviderId { get; set; }
    public required List<ProductImage> Images { get; set; }
    public required Provider Provider { get; set; }
}
