namespace ProductService.DAL.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ProductArticle { get; set; }
    public string Url { get; set; }
    public Product Product { get; set; }
}
