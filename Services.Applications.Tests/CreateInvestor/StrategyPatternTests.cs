using Services.Applications.Strategies.InvestorStrategy;
using Services.Applications.Tests.CreateInvestor.Fixtures;
using Services.Common.Abstractions.Model;
using Xunit;

namespace Services.Applications.Tests.CreateInvestor;

public class StrategyPatternTests(StrategyPatternFixture fixture) : IClassFixture<StrategyPatternFixture>
{
    //Application_for_ProductOne_creates_Investor_in_AdministratorOne
    [Fact]
    public void InvestorContext_ProductOne_CreatesInvestorUsingAdministratorOne()
    {
        
        // Arrange and Act 
        var investorCreator = fixture.InvestorContext.GetInvestorCreator(ProductCode.ProductOne);
        // Assert
        Assert.NotNull(investorCreator);
        Assert.IsType<ProductOneInvestorCreator>(investorCreator);
    }

    [Fact]
    public void InvestorContext_ProductTwo_CreatesInvestorUsingAdministratorTwo()
    {
        // Arrange and Act
        var investorCreator = fixture.InvestorContext.GetInvestorCreator(ProductCode.ProductTwo);
        // Assert
        Assert.NotNull(investorCreator);
        Assert.IsType<ProductTwoInvestorCreator>(investorCreator);
    }
}
