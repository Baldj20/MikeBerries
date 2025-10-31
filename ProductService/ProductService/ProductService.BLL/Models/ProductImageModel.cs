namespace ProductService.BLL.Models;

public class ProductImageModel
{
    public required string Url { get; set; }
    public required ProductModel Product { get; set; }
}
