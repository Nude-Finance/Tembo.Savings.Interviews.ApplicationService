using Moq;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Abstractions;
using Services.Applications.ApplicationProcessors;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;
using Shouldly;
using Xunit;

namespace Services.Applications.Tests.ApplicationProcessorTests;

public class ProductOneApplicationProcessorTests
{
    private readonly Mock<IApplicationValidator<ProductOne>> _applicationValidator = new();
    private readonly Mock<IKycService> _kycService = new();
    private readonly Mock<IAdministrationService> _administratorOneService = new();
    
    private readonly ProductOneApplicationProcessor _sut;

    public ProductOneApplicationProcessorTests()
    {
        _sut = new ProductOneApplicationProcessor(_applicationValidator.Object, _kycService.Object, _administratorOneService.Object);
    }
    
    [Theory, DateOfBirthAutoData]
    public async Task Invalid_application_returns_error(Application<ProductOne> application, Error validationError)
    {
        //Arrange
        _applicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Failure(validationError));
        
        //Act
        var result = await _sut.Process(application);
        
        //Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(validationError);
        
        _applicationValidator.Verify(x => x.Validate(application), Times.Once);
        _kycService.Verify(x => x.GetKycReportAsync(application.Applicant), Times.Never);
        _administratorOneService.Verify(x => x.CreateInvestor(It.IsAny<CreateInvestorRequest>()), Times.Never);
    }

    [Theory, DateOfBirthAutoData]
    public async Task Kyc_failed_application_returns_error(Application<ProductOne> application, Error kycError)
    {
        //Arrange
        _applicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Success());

        _kycService.Setup(x => x.GetKycReportAsync(application.Applicant))
            .ReturnsAsync(Result.Failure<KycReport>(kycError));
        
        //Act
        var result = await _sut.Process(application);
        
        //Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(kycError);
        
        _applicationValidator.Verify(x => x.Validate(application), Times.Once);
        _kycService.Verify(x => x.GetKycReportAsync(application.Applicant), Times.Once);
        _administratorOneService.Verify(x => x.CreateInvestor(It.IsAny<CreateInvestorRequest>()), Times.Never);
    }
    
    //ProductOneTests.cs - Application_for_ProductOne_creates_Investor_in_AdministratorOne()
    [Theory, DateOfBirthAutoData]
    public async Task Valid_application_creates_investor_in_AdministratorOne(Application<ProductOne> application, KycReport kycReport)
    {
        //Arrange
        _applicationValidator.Setup(x => x.Validate(application))
            .Returns(Result.Success());
        
        _kycService.Setup(x => x.GetKycReportAsync(application.Applicant))
            .ReturnsAsync(() => Result.Success<KycReport>(kycReport));
        
        _administratorOneService.Setup(x => x.CreateInvestor(It.Is<CreateInvestorRequest>(req =>
                req.FirstName == application.Applicant.Forename &&
                req.LastName == application.Applicant.Surname)))
            .Returns(new CreateInvestorResponse())
            .Verifiable();
        
        //Act  
        await _sut.Process(application);
        
        //Assert
        _applicationValidator.Verify(x => x.Validate(application), Times.Once);
        _kycService.Verify(x => x.GetKycReportAsync(application.Applicant), Times.Once);
        _administratorOneService.Verify(x => x.CreateInvestor(It.IsAny<CreateInvestorRequest>()), Times.Once);
    }
}