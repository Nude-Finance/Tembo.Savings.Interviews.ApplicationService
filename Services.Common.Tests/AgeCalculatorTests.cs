namespace Services.Common.Tests;

using Abstractions.Validators;

public class AgeCalculatorTests
    {
        private readonly IAgeCalculator _ageCalculator = new AgeCalculator();

        [Fact]
        public void CalculateAgeFromDateOfBirth_BirthdayToday_ReturnsCorrectAge()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dateOfBirth = today.AddYears(-30);

            // Act
            var age = _ageCalculator.CalculateAgeFromDateOfBirth(dateOfBirth);

            // Assert
            Assert.Equal(30, age);
        }

        [Fact]
        public void CalculateAgeFromDateOfBirth_BirthdayNotYetOccuredThisYear_ReturnsCorrectAge()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dateOfBirth = today.AddYears(-30).AddDays(1);

            // Act
            var age = _ageCalculator.CalculateAgeFromDateOfBirth(dateOfBirth);

            // Assert
            Assert.Equal(29, age);
        }

        [Fact]
        public void CalculateAgeFromDateOfBirth_BirthdayAlreadyOccurredThisYear_ReturnsCorrectAge()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dateOfBirth = today.AddYears(-30).AddDays(-1);

            // Act
            var age = _ageCalculator.CalculateAgeFromDateOfBirth(dateOfBirth);

            // Assert
            Assert.Equal(30, age);
        }

        [Fact]
        public void CalculateAgeFromDateOfBirth_FutureDateOfBirth_ThrowsArgumentException()
        {
            // Arrange
            var futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _ageCalculator.CalculateAgeFromDateOfBirth(futureDate));
        }

        [Fact]
        public void CalculateAgeFromDateOfBirth_BornToday_ReturnsZeroAge()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var age = _ageCalculator.CalculateAgeFromDateOfBirth(today);

            // Assert
            Assert.Equal(0, age);
        }
    }
