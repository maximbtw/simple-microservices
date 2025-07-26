using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Utilities.CustomAttributes;

public class NotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is ICollection collectionValue)
        {
            if (collectionValue.Count == 0)
            {
                return new ValidationResult(ErrorMessage ?? "The collection must not be empty.");
            }
        }
        else if (value is string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(ErrorMessage ?? "The string must not be empty.");
            }
        }
        else if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "The value must not be null.");
        }

        return ValidationResult.Success!;
    }
}