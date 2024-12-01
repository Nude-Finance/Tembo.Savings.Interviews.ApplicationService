namespace Services.Common.Tests;

using Abstractions.Validators;
using FluentAssertions;
using Moq;

public class AgeValidatorTests
{
	private readonly Mock<IAgeCalculator> _ageCalculatorMock;
    private readonly IAgeValidator _ageValidator;

    public AgeValidatorTests()
    {
        _ageCalculatorMock = new Mock<IAgeCalculator>();
        _ageValidator = new AgeValidator(_ageCalculatorMock.Object);
    }

    [Fact]
    public void IsAgeValid_ShouldReturnTrue_WhenAgeIsWithinRange()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var currentAge = 25;
        _ageCalculatorMock.Setup(x => x.CalculateAgeFromDateOfBirth(dateOfBirth)).Returns(currentAge);

        // Act
        var result = _ageValidator.IsAgeValid(dateOfBirth, 18, 30);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAgeValid_ShouldReturnFalse_WhenAgeIsBelowMinAge()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2010, 1, 1);
        var currentAge = 14;
        _ageCalculatorMock.Setup(x => x.CalculateAgeFromDateOfBirth(dateOfBirth)).Returns(currentAge);

        // Act
        var result = _ageValidator.IsAgeValid(dateOfBirth, 18, 30);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAgeValid_ShouldReturnFalse_WhenAgeIsGreaterThanMaxAge()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2000, 1, 1);
        var currentAge = 25;
        _ageCalculatorMock.Setup(x => x.CalculateAgeFromDateOfBirth(dateOfBirth)).Returns(currentAge);

        // Act
        var result = _ageValidator.IsAgeValid(dateOfBirth, 18, 24);

        // Assert
        result.Should().BeFalse();
    }

}
