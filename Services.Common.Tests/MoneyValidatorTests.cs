namespace Services.Common.Tests;

using Abstractions.Model;
using Abstractions.Validators;
using FluentAssertions;

public class MoneyValidatorTests
    {
        private readonly MoneyValidator _moneyValidator = new MoneyValidator();

        [Fact]
        public void IsMoneyValid_ShouldThrowArgumentException_WhenNeitherMinNorMaxMoneyIsProvided()
        {
            // Arrange
            var money = new Money("USD", 100);

            // Act & Assert
            Action act = () => _moneyValidator.IsMoneyValid(money, null, null);
            act.Should().Throw<ArgumentException>().WithMessage("Either minMoney or maxMoney must be specified.");
        }

        [Fact]
        public void IsMoneyValid_ShouldThrowInvalidOperationException_WhenMinMoneyCurrencyDoesNotMatch()
        {
            // Arrange
            var money = new Money("USD", 100);
            var minMoney = new Money("EUR", 50);

            // Act & Assert
            Action act = () => _moneyValidator.IsMoneyValid(money, minMoney, null);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Currency mismatch: Money currency (USD) and minMoney currency (EUR) must match.");
        }

        [Fact]
        public void IsMoneyValid_ShouldThrowInvalidOperationException_WhenMaxMoneyCurrencyDoesNotMatch()
        {
            // Arrange
            var money = new Money("USD", 100);
            var maxMoney = new Money("EUR", 150);

            // Act & Assert
            Action act = () => _moneyValidator.IsMoneyValid(money, null, maxMoney);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Currency mismatch: Money currency (USD) and maxMoney currency (EUR) must match.");
        }

        [Fact]
        public void IsMoneyValid_ShouldReturnFalse_WhenMoneyAmountIsLessThanMinAmount()
        {
            // Arrange
            var money = new Money("USD", 50);
            var minMoney = new Money("USD", 100);

            // Act
            var result = _moneyValidator.IsMoneyValid(money, minMoney, null);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsMoneyValid_ShouldReturnFalse_WhenMoneyAmountIsGreaterThanMaxAmount()
        {
            // Arrange
            var money = new Money("USD", 200);
            var maxMoney = new Money("USD", 150);

            // Act
            var result = _moneyValidator.IsMoneyValid(money, null, maxMoney);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsMoneyValid_ShouldReturnTrue_WhenMoneyIsWithinMinAndMaxBounds()
        {
            // Arrange
            var money = new Money("USD", 100);
            var minMoney = new Money("USD", 50);
            var maxMoney = new Money("USD", 150);

            // Act
            var result = _moneyValidator.IsMoneyValid(money, minMoney, maxMoney);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsMoneyValid_ShouldReturnTrue_WhenMoneyIsExactlyEqualToMinAmount()
        {
            // Arrange
            var money = new Money("USD", 100);
            var minMoney = new Money("USD", 100);

            // Act
            var result = _moneyValidator.IsMoneyValid(money, minMoney, null);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsMoneyValid_ShouldReturnTrue_WhenMoneyIsExactlyEqualToMaxAmount()
        {
            // Arrange
            var money = new Money("USD", 150);
            var maxMoney = new Money("USD", 150);

            // Act
            var result = _moneyValidator.IsMoneyValid(money, null, maxMoney);

            // Assert
            result.Should().BeTrue();
        }
    }
