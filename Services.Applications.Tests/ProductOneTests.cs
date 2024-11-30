using FluentAssertions;
using Moq;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;
using Xunit;

namespace Services.Applications.Tests;

public class ProductOneTests
{
    private readonly Mock<IAdministrationService> _administratorOneService;
    
    private readonly ProductOneApplicationProcessor _sut;

    public ProductOneTests()
    {
        _administratorOneService = new Mock<IAdministrationService>();
        _sut = new ProductOneApplicationProcessor(_administratorOneService.Object);
    }
    
    [Fact]
    public async Task Application_for_ProductOne_creates_Investor_in_AdministratorOne()
    {
        //Arrange
        _administratorOneService.Setup(x => x.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(new CreateInvestorResponse())
            .Verifiable();    
            
        var productOneApplication  = new Mock<Application<ProductOne>>().Object;
        
        //Act  
        await _sut.Process(productOneApplication);
        
        //Assert
        _administratorOneService.Verify();
    }
}