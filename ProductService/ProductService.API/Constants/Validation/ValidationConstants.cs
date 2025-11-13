namespace ProductService.API.Constants.Validation;

public static class ValidationConstants
{
    public const string PRICE_MUST_BE_GREATER_THAN_ZERO = "product price must be greater than zero";
    public const string VALUE_CANNOT_BE_NULL = "this attribute cannot have null value";
    public const string NAME_IS_TOO_LONG = "name should contain less than 100 characters";
    public const string EMAIL_IS_NOT_VALID = "email is not valid";
    public const string FILE_IS_REQUIRED = "image file is required";
}
