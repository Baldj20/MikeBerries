using FluentValidation;
using ProductService.API.Constants.Validation;
using ProductService.BLL.DTO;

namespace ProductService.API.Validators;

public class ProviderDtoValidator : AbstractValidator<ProviderDto>
{
    public ProviderDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(ValidationConstants.NAME_IS_TOO_LONG);
        RuleFor(p => p.Email)
            .Matches("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$").WithMessage(ValidationConstants.EMAIL_IS_NOT_VALID);
    }
}
