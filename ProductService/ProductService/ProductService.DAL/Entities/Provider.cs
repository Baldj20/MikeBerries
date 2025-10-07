namespace ProductService.DAL.Entities;

public class Provider
{
    public string Email { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
