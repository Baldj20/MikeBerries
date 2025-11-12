namespace ProductService.DAL.Entities;

public class Provider : BaseEntity
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
