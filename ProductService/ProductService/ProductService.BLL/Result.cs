namespace ProductService.BLL;

public record Result
{
    protected Result(bool isSuccess, CustomError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; set; }
    public CustomError? Error { get; set; }

    public static Result Success() => new(true, null);
    public static Result Failure(CustomError error) => new(false, error);
}

public record Result<T> : Result
{
    public T? Value { get; }
    public Result(T value) : base(true, null) => Value = value;
    public Result(CustomError error) : base(false, error) { }
}
