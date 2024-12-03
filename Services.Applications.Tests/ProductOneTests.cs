using Moq;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Application.Domain;
using Services.Application.Domain.Adapters;
using Services.Application.Domain.Interfaces;
using Services.Application.Domain.Services;
using Services.Common.Abstractions;
using Services.Common.Model;
using Xunit;

namespace Services.Applications.Tests;

public class ProductOneTests
{
    public AdministratorFactory _administratorFactory { get; set; }
    public Mock<IKycService> _mockKycService { get; set; }
    public Mock<IBus> _mockBus { get; set; }
    public new Mock<Services.AdministratorOne.Abstractions.IAdministrationService> _mockServiceAdministratorOne { get; set; }
    
    public ProductOneTests()
    {
        _mockServiceAdministratorOne = new Mock<Services.AdministratorOne.Abstractions.IAdministrationService>();

        var administratorOneAdapter = new AdministratorOneAdapter(_mockServiceAdministratorOne.Object);

        _administratorFactory = new AdministratorFactory(
            administratorOneAdapter,
            null
        );

        _mockKycService = new Mock<IKycService>();
        _mockBus = new Mock<IBus>();
    }
    
    [Fact]
    public async Task Application_for_ProductOne_creates_Investor_in_AdministratorOne()
    {
        // Arrange
        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductOne,
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                IsVerified = true
            },
            Payment = new Payment(new BankAccount(), new Money("USD", 1000))
        };

        var userGuid = Guid.NewGuid();
        var kycResult = new Result<KycReport>(true, null, new KycReport(userGuid, true));
        _mockKycService.Setup(k => k.GetKycReportAsync(It.IsAny<User>())).ReturnsAsync(kycResult);

        CreateInvestorResponse createInvestorResponse = new CreateInvestorResponse()
        {
            InvestorId = "InvestorId"
        };
        _mockServiceAdministratorOne.Setup(s => s.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(createInvestorResponse);

        var processingService = new ProcessingService(_administratorFactory, _mockBus.Object);

        var applicationProcessor = new ApplicationProcessor(
            _mockKycService.Object,
            _mockBus.Object,
            processingService
        );

        // Act
        await applicationProcessor.Process(application);

        // Assert
        _mockServiceAdministratorOne.Verify(s => s.CreateInvestor(It.IsAny<CreateInvestorRequest>()), Times.Once);
        _mockBus.Verify(b => b.PublishAsync(It.IsAny<DomainEvent>()), Times.Exactly(2));
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is InvestorCreated)), Times.Once);
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is ApplicationCompleted)), Times.Once);
    }

    [Theory]
    [InlineData(17)]
    [InlineData(45)]
    [InlineData(40)]
    [InlineData(-21)]
    public async Task Application_for_ProductOne_Fails_Validation_Age_OutOfRange(int age)
    {
        // Arrange
        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductOne,
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-age)),
                IsVerified = true
            },
            Payment = new Payment(new BankAccount(), new Money("GBP", 1000))
        };

        var userGuid = Guid.NewGuid();
        var kycResult = new Result<KycReport>(true, null, new KycReport(userGuid, true));
        _mockKycService.Setup(k => k.GetKycReportAsync(It.IsAny<User>())).ReturnsAsync(kycResult);

        CreateInvestorResponse createInvestorResponse = new CreateInvestorResponse()
        {
            InvestorId = "InvestorId"
        };
        _mockServiceAdministratorOne.Setup(s => s.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(createInvestorResponse);

        var processingService = new ProcessingService(_administratorFactory, _mockBus.Object);

        var applicationProcessor = new ApplicationProcessor(
            _mockKycService.Object,
            _mockBus.Object,
            processingService
        );

        // Act
        await applicationProcessor.Process(application);

        // Assert
        _mockBus.Verify(b => b.PublishAsync(It.IsAny<DomainEvent>()), Times.Exactly(1));
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is ApplicationNotValid)), Times.Once);
    }
    
    [Theory]
    [InlineData(17, 0.99)]
    [InlineData(45, 0.99)]
    [InlineData(40, 0.99)]
    [InlineData(-21, 0.99)]
    [InlineData(25, 0.98)]
    [InlineData(25, 0.00)]
    public async Task Application_for_ProductOne_Fails_Validation(int age, decimal amount)
    {
        // Arrange
        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductOne,
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-age)),
                IsVerified = true
            },
            Payment = new Payment(new BankAccount(), new Money("GBP", amount))
        };

        var userGuid = Guid.NewGuid();
        var kycResult = new Result<KycReport>(true, null, new KycReport(userGuid, true));
        _mockKycService.Setup(k => k.GetKycReportAsync(It.IsAny<User>())).ReturnsAsync(kycResult);

        CreateInvestorResponse createInvestorResponse = new CreateInvestorResponse()
        {
            InvestorId = "InvestorId"
        };
        _mockServiceAdministratorOne.Setup(s => s.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(createInvestorResponse);

        var processingService = new ProcessingService(_administratorFactory, _mockBus.Object);

        var applicationProcessor = new ApplicationProcessor(
            _mockKycService.Object,
            _mockBus.Object,
            processingService
        );

        // Act
        await applicationProcessor.Process(application);

        // Assert
        _mockBus.Verify(b => b.PublishAsync(It.IsAny<DomainEvent>()), Times.Exactly(1));
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is ApplicationNotValid)), Times.Once);
    }
}