using Services.Applications.Strategies.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.InvestorStrategy.Validators;

public class ProductOneInvestorValidation : IInvestorValidation
{
    public void Validate(Application application)
    {
        var age = CalculateAge(application.Applicant.DateOfBirth);

        if (age is < 18 or > 39)
        {
            throw new InvalidOperationException("ProductOne is only available to people aged 18 to 39.");
        }

        if (application.Payment.Amount.Amount < 0.99m)
        {
            throw new InvalidOperationException("Minimum payment for ProductOne is £0.99.");
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
