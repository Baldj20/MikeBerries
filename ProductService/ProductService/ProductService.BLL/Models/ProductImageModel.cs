using ProductService.DAL.Entities;

namespace ProductService.BLL.Models;

public class ProductImageModel
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public Product Product { get; set; }
}
