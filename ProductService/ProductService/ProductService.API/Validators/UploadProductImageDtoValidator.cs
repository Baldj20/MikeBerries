using FluentValidation;
using ProductService.API.Constants.Validation;
using ProductService.BLL.DTO;

namespace ProductService.API.Validators;

public class UploadProductImageDtoValidator : AbstractValidator<UploadProductImageDTO>
{
    public UploadProductImageDtoValidator()
    {
        RuleFor(i => i.Image)
            .NotNull().WithMessage(ValidationConstants.FILE_IS_REQUIRED);
    }
}
