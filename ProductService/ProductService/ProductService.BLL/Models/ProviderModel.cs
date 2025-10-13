using ProductService.DAL.Entities;

namespace ProductService.BLL.Models;

public class ProviderModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } = new();
}
