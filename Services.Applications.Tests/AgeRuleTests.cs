using Services.Application.Domain.Rules;
using Services.Common.Model;
using Xunit;

namespace Services.Applications.Tests;

public class AgeRuleTests
{
    [Theory]
    [InlineData(18, 39, 25, true)]
    [InlineData(18, 39, 18, true)]
    [InlineData(18, 39, 39, true)]
    [InlineData(18, 39, 17, false)]
    [InlineData(18, 39, 40, false)]
    public void AgeRule_Should_Validate_Age_Correctly(int minAge, int maxAge, int applicantAge, bool expected)
    {
        // Arrange
        var rule = new AgeRule(minAge, maxAge);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-applicantAge))
            }
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AgeRule_Should_Return_False_For_Applicant_Over_200_Years_Old()
    {
        // Arrange
        var rule = new AgeRule(18, 200);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-201))
            }
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AgeRule_Should_Return_True_For_Applicant_Under_Min_Age()
    {
        // Arrange
        var rule = new AgeRule(int.MinValue, 45);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-42))
            }
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AgeRule_Should_Return_True_For_Applicant_Within_Max_Age()
    {
        // Arrange
        var rule = new AgeRule(18, int.MaxValue);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25))
            }
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AgeRule_Should_Return_True_For_Applicant_Within_Min_And_Max_Age()
    {
        // Arrange
        var rule = new AgeRule(int.MinValue, int.MaxValue);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-19))
            }
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AgeRule_Should_Throw_For_Misconfigured_Age_Rule()
    {
        // Arrange
        var rule = new AgeRule(50, 18);
        var application = new Common.Model.Application
        {
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-19))
            }
        };

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => rule.IsValid(application));
    }
}