using Services.Applications.Strategies.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.InvestorStrategy.Validators;

public class ProductTwoInvestorValidation : IInvestorValidation
{
    public void Validate(Application application)
    {
        var age = CalculateAge(application.Applicant.DateOfBirth);

        if (age < 18)
        {
            throw new InvalidOperationException("ProductTwo is only available for 18 years old or older.");
        }

        if (application.Payment.Amount.Amount < 0.99m)
        {
            throw new InvalidOperationException("Minimum payment for ProductTwo is £0.99.");
        }
    }

    private int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;
        return age;
    }
}
