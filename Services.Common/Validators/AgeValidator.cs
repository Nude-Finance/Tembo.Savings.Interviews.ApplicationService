namespace Services.Common.Abstractions.Validators;

public interface IAgeValidator
{
	bool IsAgeValid(DateOnly dateOfBirth, int minAge = int.MinValue, int maxAge = int.MaxValue);
}

public class AgeValidator(IAgeCalculator ageCalculator) : IAgeValidator
{
	public bool IsAgeValid(DateOnly dateOfBirth, int minAge= int.MinValue, int maxAge = int.MaxValue)
	{
		var age = ageCalculator.CalculateAgeFromDateOfBirth(dateOfBirth);
		return age >= minAge && age < maxAge;
	}
}
