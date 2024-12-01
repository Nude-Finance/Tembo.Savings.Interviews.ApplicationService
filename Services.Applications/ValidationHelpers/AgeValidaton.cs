using Services.Common.Abstractions.Model;

namespace Services.Applications.ValidationHelpers;

internal static class AgeValidation
{
    private static Result ValidationFailure(string validationError)
        => Result.Failure(new Error("Validation Error", "422", validationError));

    /// <summary>
    /// Validates that a given DateOnly object represents a birthdate for a person at or over
    /// a given number of years of age.
    /// </summary>
    /// <param name="yearsOld"></param>
    /// <param name="dob"></param>
    /// <returns>Success or an Error with a validation error message</returns>
    internal static Result MinAge(int yearsOld, DateOnly dob)
    {
        var minAgeDob = DateOnly.FromDateTime(DateTime.Today.AddYears(yearsOld * -1));
        var isMinAgeOrOver = minAgeDob.CompareTo(dob) >= 0;
        
        if (!isMinAgeOrOver)
        {
            return ValidationFailure($"Must be {yearsOld} years or older.");
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates that a given DateOnly object represents a birthdate for a person at or under
    /// a given number of years of age.
    /// </summary>
    /// <param name="yearsOld"></param>
    /// <param name="dob"></param>
    /// <returns>Success or an Error with a validation error message</returns>
    internal static Result MaxAge(int yearsOld, DateOnly dob)
    {
        var maxAgeDob = DateOnly.FromDateTime(DateTime.Today.AddYears(yearsOld * -1));
        var isMaxAgeOrUnder = maxAgeDob.CompareTo(dob) <= 0;
        
        if (!isMaxAgeOrUnder)
        {
            return ValidationFailure($"Must be {yearsOld} years old or less.");
        }

        return Result.Success();
    }
}