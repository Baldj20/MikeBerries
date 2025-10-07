namespace ProductService.DAL.Entities;

public class ProductImage
{
    public int ID { get; set; }
    public Product Product { get; set; }
    public string ProductArticle { get; set; }
    public string URL { get; set; }
}
