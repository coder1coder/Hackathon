using FluentValidation;
using FluentValidation.Internal;

namespace Hackathon.BL.Validation.Extensions;

public static class ValidationExtensions
{
    public static async Task ValidateAndThrowAsync<T>(
        this IValidator<T> validator,
        T instance,
        Action<ValidationStrategy<T>> options)
    {
        var result = await validator.ValidateAsync(instance, options);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}
