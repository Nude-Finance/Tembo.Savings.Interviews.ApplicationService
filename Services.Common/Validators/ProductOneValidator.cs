namespace Services.Common.Abstractions.Validators;

using Abstractions;
using Model;

public class ProductOneValidator(IAgeValidator ageValidator, IMoneyValidator moneyValidator) : IProductValidator
{
	public ProductCode SupportedProductCode => ProductCode.ProductOne;
	public bool Validate(Application application)
	{
		if (!ageValidator.IsAgeValid(application.Applicant.DateOfBirth, 18, 39))
		{
			return false;
		}
		
		var minMoney = new Money(application.Payment.Amount.Currency, 0.99m);

		if (!moneyValidator.IsMoneyValid(application.Payment.Amount, minMoney))
		{
			return false;
		}
		return true;
	}
}
