using System.ComponentModel.DataAnnotations;

namespace Utilities.CustomAttributes;

public class PositiveAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? obj, ValidationContext validationContext)
    {
        if (obj is null)
        {
            return ValidationResult.Success!;
        }
        
        if (obj is decimal and <= 0)
        {
            return new ValidationResult(ErrorMessage ?? "Value must be positive.");
        }
        
        if (obj is int and <= 0)
        {
            return new ValidationResult(ErrorMessage ?? "Value must be positive.");
        }
        
        if (obj is double and <= 0)
        {
            return new ValidationResult(ErrorMessage ?? "Value must be positive.");
        }
        
        if (obj is float and <= 0)
        {
            return new ValidationResult(ErrorMessage ?? "Value must be positive.");
        }

        return ValidationResult.Success!;
    }
}