using Moq;
using Services.AdministratorTwo.Abstractions;
using Services.Applications.Abstractions;
using Services.Applications.ApplicationProcessors;
using Services.Applications.ApplicationValidators;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;
using Shouldly;
using Xunit;

namespace Services.Applications.Tests.ApplicationProcessorTests;

public class ProductTwoApplicationProcessorTests
{
    private readonly Mock<IApplicationValidator<ProductTwo>> _productTwoApplicationValidator = new();
    private readonly Mock<IKycService> _kycService = new();
    private readonly Mock<IAdministrationService> _administratorTwoService = new();
    
    private readonly ProductTwoApplicationProcessor _sut;

    public ProductTwoApplicationProcessorTests()
    {
        _sut = new ProductTwoApplicationProcessor(
            _productTwoApplicationValidator.Object, 
            _kycService.Object, 
            _administratorTwoService.Object);
    }

    [Theory, DateOfBirthAutoData]
    public async Task Invalid_application_returns_error(
        Application<ProductTwo> application,
        Error validationError)
    {
        //Arrange
        _productTwoApplicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Failure(validationError));
        
        //Act
        var result = await _sut.Process(application);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(validationError);
        
        _productTwoApplicationValidator.Verify(x => x.Validate(application), Times.Once);
        _kycService.Verify(x => x.GetKycReportAsync(It.IsAny<User>()), Times.Never);
        
        _administratorTwoService.Verify(x => 
            x.CreateInvestorAsync(It.IsAny<User>()), Times.Never);
        _administratorTwoService.Verify(x => 
            x.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()), Times.Never);
        _administratorTwoService.Verify(x => 
            x.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()), Times.Never);
    }
    
    [Theory, DateOfBirthAutoData]
    public async Task Kyc_failed_application_returns_error(
        Application<ProductTwo> application,
        Error kycError)
    {
        //Arrange
        _productTwoApplicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Success());
        
        _kycService.Setup(x => x.GetKycReportAsync(application.Applicant))
            .ReturnsAsync(Result.Failure<KycReport>(kycError));
        
        //Act
        var result = await _sut.Process(application);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(kycError);
        
        _productTwoApplicationValidator.Verify();
        _kycService.Verify();
        
        _administratorTwoService.Verify(x => 
            x.CreateInvestorAsync(It.IsAny<User>()), Times.Never);
        _administratorTwoService.Verify(x => 
            x.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()), Times.Never);
        _administratorTwoService.Verify(x => 
            x.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()), Times.Never);
    }
    
    [Theory, DateOfBirthAutoData]
    public async Task Valid_Application_Creates_Investor_Account_and_Payment_in_AdministratorTwo(
        Application<ProductTwo> application,
        KycReport kycReport,
        Guid createdInvestorId, 
        Guid createdAccountId, 
        Guid createdPaymentId)
    {
        //Arrange
        _productTwoApplicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Success());

        _kycService.Setup(x => x.GetKycReportAsync(application.Applicant))
            .ReturnsAsync(Result.Success(kycReport));
        
        _administratorTwoService.Setup(x => x.CreateInvestorAsync(application.Applicant))
            .ReturnsAsync(Result.Success(createdInvestorId))
            .Verifiable();

        _administratorTwoService.Setup(x =>
                x.CreateAccountAsync(createdInvestorId, application.Product.ProductCode))
            .ReturnsAsync(Result.Success(createdAccountId))
            .Verifiable();
        
        _administratorTwoService.Setup(x => x.ProcessPaymentAsync(createdAccountId, application.Payment))
            .ReturnsAsync(Result.Success(createdPaymentId))
            .Verifiable();
            
        //Act  
        await _sut.Process(application);
        
        //Assert
        _productTwoApplicationValidator.Verify();
        _kycService.Verify();
        _administratorTwoService.Verify();
    }
}