namespace ProductService.BLL;

internal class Result<T>
{
    private readonly T? _value;
    public T Value
    {
        get
        {
            if (!IsSuccess) throw new InvalidOperationException("there is no value for failure");
            return _value;
        }
    }
    public bool IsSuccess { get; }
    public CustomError Error { get; }

    private Result(T value)
    {
        _value = value;
        IsSuccess = true;
        Error = CustomError.None;
    }

    private Result(CustomError error)
    {
        if (error == CustomError.None) throw new ArgumentException("invalid error");

        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Failure(CustomError error)
    {
        return new Result<T>(error);
    }
}
