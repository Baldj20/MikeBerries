using FluentValidation;
using ProductService.API.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ProductService.API.Configurations;

public static class ValidatorsConfigurations
{
    public static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.EnableFormBindingSourceAutomaticValidation = true;
            configuration.EnableQueryBindingSourceAutomaticValidation = true;
        });
    }
}
