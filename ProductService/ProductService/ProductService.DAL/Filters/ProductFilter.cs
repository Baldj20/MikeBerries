using ProductService.DAL.Builders;
using ProductService.DAL.Entities;
using ProductService.DAL.Interfaces.Filters;

namespace ProductService.DAL.Filters;

public record ProductFilter : IFilter<Product>
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

    public IQueryable<Product> Apply(IQueryable<Product> query)
    {
        return query.WithTitle(Title)
                    .WithProvider(Provider)
                    .HasMinPrice(MinPrice)
                    .HasMaxPrice(MaxPrice);
    }
}
