namespace ProductService.DAL.Entities;

public class ProductImage : BaseEntity
{
    public required string Url { get; set; }
    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
}
