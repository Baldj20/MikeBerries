namespace ProductService.DAL.Interfaces.Filters;

public interface IFilter<T>
{
    public IQueryable<T> Apply(IQueryable<T> query);
}
