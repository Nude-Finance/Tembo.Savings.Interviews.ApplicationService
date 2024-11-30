using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using Services.AdministratorTwo.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;
using Xunit;

namespace Services.Applications.Tests;

public class ProductTwoTests
{
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<IAdministrationService> _administratorTwoService = new Mock<IAdministrationService>();
    private readonly Application<ProductTwo> _application;
    
    private readonly ProductTwoApplicationProcessor _sut;

    public ProductTwoTests()
    {
        var applicant = _fixture.Build<User>()
            .With(x => x.DateOfBirth, DateOnly.Parse("1994-01-01"))
            .Create();
        
        _application = _fixture.Build<Application<ProductTwo>>()
            .With(x => x.Applicant, applicant)
            .Create();
            
        _sut = new ProductTwoApplicationProcessor(_administratorTwoService.Object);
    }
    
    [Theory, AutoData]
    public async Task Application_for_ProductTwo_creates_Investor_Account_and_Payment_in_AdministratorTwo(
        Guid createdInvestorId, 
        Guid createdAccountId, 
        Guid createdPaymentId)
    {
        //Arrange
        _administratorTwoService.Setup(x => x.CreateInvestorAsync(_application.Applicant))
            .ReturnsAsync(Result.Success(createdInvestorId))
            .Verifiable();

        _administratorTwoService.Setup(x =>
                x.CreateAccountAsync(createdInvestorId, _application.Product.ProductCode))
            .ReturnsAsync(Result.Success(createdAccountId))
            .Verifiable();
        
        _administratorTwoService.Setup(x => x.ProcessPaymentAsync(createdAccountId, _application.Payment))
            .ReturnsAsync(Result.Success(createdPaymentId))
            .Verifiable();
            
        //Act  
        await _sut.Process(_application);
        
        //Assert
        _administratorTwoService.Verify();
    }
}