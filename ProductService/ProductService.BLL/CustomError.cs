namespace ProductService.BLL;

public sealed record CustomError(string message)
{
    public readonly static CustomError None = new CustomError(string.Empty);

    public static CustomError ResourceNotFound<T>()
    {
        return new CustomError($"{typeof(T).Name} not found");
    }

    public static CustomError ResourceForbidden()
    {
        return new CustomError($"You dont have access to this resource");
    }
}
