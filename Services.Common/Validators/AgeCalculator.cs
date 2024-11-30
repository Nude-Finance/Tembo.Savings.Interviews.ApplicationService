namespace Services.Common.Abstractions.Validators;

public interface IAgeCalculator
{
	int CalculateAgeFromDateOfBirth(DateOnly dateOfBirth);
}

public class AgeCalculator:IAgeCalculator
{
	public int CalculateAgeFromDateOfBirth(DateOnly dateOfBirth)
	{
		var today = DateOnly.FromDateTime(DateTime.Now);

		ArgumentOutOfRangeException.ThrowIfGreaterThan(dateOfBirth, today);

		var age = today.Year - dateOfBirth.Year;
		
		if (today < dateOfBirth.AddYears(age))
		{
			age--;
		}

		return age;
	}
}
