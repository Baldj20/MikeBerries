namespace ProductService.BLL;

public sealed record CustomError(string code, string message)
{
    private static readonly string ResourceNotFoundCode = "ResourceNotFound";
    private static readonly string ResourceAlreadyExistsCode = "ResourceAlreasyExists";

    public readonly static CustomError None = new CustomError(string.Empty, string.Empty);

    public static CustomError ResourceNotFound(string message)
    {
        return new CustomError(message, ResourceNotFoundCode);
    }
    public static CustomError ResourceAlreadyExists(string message)
    {
        return new CustomError(message, ResourceAlreadyExistsCode);
    }
}
