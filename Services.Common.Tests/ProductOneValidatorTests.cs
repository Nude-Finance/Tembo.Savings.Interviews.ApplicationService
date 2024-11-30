namespace Services.Common.Tests;

using Abstractions.Model;
using FluentAssertions;
using Moq;

public class ProductOneValidatorTests : BaseProductValidatorTests
{
	[Fact]
	public void Validate_ShouldReturnFalse_WhenApplicantAgeIsInvalid()
	{
		// Arrange
		AgeValidatorMock.Setup(x => x.IsAgeValid(It.IsAny<DateOnly>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);

		// Act
		var result = ProductOneValidator.Validate(Application);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Validate_ShouldReturnFalse_WhenPaymentAmountIsLessThanMinMoney()
	{
		// Arrange
		AgeValidatorMock.Setup(x => x.IsAgeValid(It.IsAny<DateOnly>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
		MoneyValidatorMock.Setup(x => x.IsMoneyValid(It.IsAny<Money>(), It.IsAny<Money>(), It.IsAny<Money?>())).Returns(false);

		// Act
		var result = ProductOneValidator.Validate(Application);

		// Assert
		result.Should().BeFalse();
	}

	[Fact]
	public void Validate_ShouldReturnTrue_WhenApplicantAgeAndPaymentAreValid()
	{
		// Arrange
		AgeValidatorMock.Setup(x => x.IsAgeValid(It.IsAny<DateOnly>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);
		MoneyValidatorMock.Setup(x => x.IsMoneyValid(It.IsAny<Money>(), It.IsAny<Money>(), It.IsAny<Money?>())).Returns(true);

		// Act
		var result = ProductOneValidator.Validate(Application);

		// Assert
		result.Should().BeTrue();
	}


}
