namespace ProductService.DAL.Filters;

public class ProviderFilter
{
    public string? Name { get; set; }
    public string? Product { get; set; }
    public void Deconstruct(out string? name, out string? product)
    {
        name = Name;
        product = Product;
    }
}
