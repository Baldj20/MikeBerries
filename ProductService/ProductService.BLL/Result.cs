namespace ProductService.BLL;

public record Result
{
    public Result() { }
    protected Result(bool isSuccess, CustomError? error, int statusCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        StatusCode = statusCode;
    }

    public bool IsSuccess { get; set; }
    public CustomError? Error { get; set; }
    public int StatusCode { get; set; }

    public static Result Success(int statusCode) => new(true, null, statusCode);
    public static Result Failure(CustomError error, int statusCode) => new(false, error, statusCode);
}

public record Result<T> : Result
{
    public T? Value { get; init; }
    public Result(T value, int statusCode) : base(true, null, statusCode) => Value = value;
    public Result(CustomError error, int statusCode) : base(false, error, statusCode) { }
    public Result() { }
}
