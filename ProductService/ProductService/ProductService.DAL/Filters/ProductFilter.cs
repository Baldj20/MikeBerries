namespace ProductService.DAL.Filters;

public record ProductFilter
{
    public string? Title { get; set; }
    public string? Provider { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public ProductFilter(string? title, string? provider, 
        decimal? minPrice, decimal? maxPrice)
    {
        Title = title;
        Provider = provider;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
    }

    public void Deconstruct(out string? title, out string? provider, out decimal? minPrice, out decimal? maxPrice)
    {
        title = Title;
        provider = Provider;
        minPrice = MinPrice;
        maxPrice = MaxPrice;
    }
}
