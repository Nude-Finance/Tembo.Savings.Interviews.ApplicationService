namespace Services.Common.Tests;

using Abstractions.Model;
using Abstractions.Validators;
using Moq;

public class BaseProductValidatorTests
{
	protected readonly Mock<IAgeValidator> AgeValidatorMock;
	protected readonly Mock<IMoneyValidator> MoneyValidatorMock;
	protected readonly ProductTwoValidator ProductOneValidator;
	protected readonly Application Application;

	protected BaseProductValidatorTests()
	{
		AgeValidatorMock = new Mock<IAgeValidator>();
		MoneyValidatorMock = new Mock<IMoneyValidator>();
		ProductOneValidator = new ProductTwoValidator(AgeValidatorMock.Object, MoneyValidatorMock.Object);
		Application = new Application
		{
			Applicant = new User
			{
				DateOfBirth = DateOnly.MinValue,
				Id = Guid.NewGuid(),
				Forename = "John",
				Nino = "AVF112233",
				Surname = "Doe",
			},
			Id = Guid.NewGuid(),
			Payment = new Payment(
				new BankAccount
				{
					AccountNumber = "123456789",
					SortCode = "44-44-44"
				},
				new Money("USD", 1000)
			),
			ProductCode = ProductCode.ProductOne
		};
	}
}
