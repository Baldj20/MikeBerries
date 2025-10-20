using FluentValidation;
using ProductService.API.Constants.Validation;
using ProductService.BLL.DTO;

namespace ProductService.API.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDTO>
{
    public CreateProductDtoValidator()
    {
        RuleFor(p => p.Title).NotNull()
            .WithMessage(ValidationConstants.VALUE_CANNOT_BE_NULL);
        RuleFor(p => p.Price)
            .NotNull().WithMessage(ValidationConstants.VALUE_CANNOT_BE_NULL)
            .GreaterThan(0).WithMessage(ValidationConstants.PRICE_MUST_BE_GREATER_THAN_ZERO);
        RuleForEach(p => p.Images).SetValidator(new UploadProductImageDtoValidator());
        RuleFor(p => p.Provider).SetValidator(new ProviderDtoValidator());
    }
}
