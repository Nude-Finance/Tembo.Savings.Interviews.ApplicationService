using FluentAssertions;
using Moq;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Strategies.InvestorStrategy;
using Services.Applications.Tests.CreateInvestor.Fixtures;
using Xunit;

namespace Services.Applications.Tests.CreateInvestor;

public class ProductOneTests(ProductOneInvestorCreatorFixture fixture): IClassFixture<ProductOneInvestorCreatorFixture>
{
    
    [Fact]
    public async Task CreateInvestorAsync_ValidApplication_CreatesInvestorSuccessfully()
    {
        // Arrange
        var creator = new ProductOneInvestorCreator(
            fixture.Validation,
            fixture.MockAdministrationService.Object
        );

        // Act
        var investorId = await creator.CreateInvestorAsync(fixture.ValidApplication);

        // Assert
        investorId.Should().NotBe(Guid.Empty);
        fixture.MockAdministrationService.Verify(service => service.CreateInvestor(It.IsAny<CreateInvestorRequest>()), Times.Once);
    }

    [Fact]
    public async Task CreateInvestorAsync_UserUnder18_ThrowsValidationException()
    {
        // Arrange
        var creator = new ProductOneInvestorCreator(
            fixture.Validation,
            fixture.MockAdministrationService.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => creator.CreateInvestorAsync(fixture.ApplicationUnder18));
        Assert.Equal("ProductOne is only available to people aged 18 to 39.", exception.Message);
    }
    
    [Fact]
    public async Task CreateInvestorAsync_InsufficientPayment_ThrowsValidationException()
    {
        // Arrange
        var creator = new ProductOneInvestorCreator(
            fixture.Validation,
            fixture.MockAdministrationService.Object
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => creator.CreateInvestorAsync(fixture.ApplicationInsufficientPayment));
        Assert.Equal("Minimum payment for ProductOne is £0.99.", exception.Message);
    }


}