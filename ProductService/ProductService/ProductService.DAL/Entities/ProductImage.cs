namespace ProductService.DAL.Entities;

public class ProductImage : BaseEntity
{
    public string Url { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}
