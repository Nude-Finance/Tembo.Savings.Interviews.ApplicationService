using Services.Application.Domain.Rules;
using Services.Common.Model;
using Xunit;

namespace Services.Applications.Tests;

public class PaymentAmountRuleTests
{
    [Theory]
    [InlineData(0.99, 1.00, true)]
    [InlineData(0.99, 0.99, true)]
    [InlineData(0.99, 0.98, false)]
    [InlineData(0.99, 0.00, false)]
    [InlineData(0.99, 1000.00, true)]
    public void PaymentAmountRule_Should_Validate_Amount_Correctly(decimal minAmount, decimal paymentAmount, bool expected)
    {
        // Arrange
        var rule = new PaymentAmountRule(new Money("GBP", minAmount));
        var application = new Common.Model.Application
        {
            Payment = new Payment(new BankAccount(), new Money("GBP", paymentAmount))
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void PaymentAmountRule_Should_Return_False_For_Negative_Amount()
    {
        // Arrange
        var rule = new PaymentAmountRule(new Money("GBP", 0.99m));
        var application = new Common.Model.Application
        {
            Payment = new Payment(new BankAccount(), new Money("GBP", -1.00m))
        };

        // Act
        var result = rule.IsValid(application);

        // Assert
        Assert.False(result);
    }
}