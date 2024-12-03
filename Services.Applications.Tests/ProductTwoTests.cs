using Moq;
using Services.AdministratorTwo.Abstractions;
using Services.Application.Domain;
using Services.Application.Domain.Adapters;
using Services.Application.Domain.Services;
using Services.Common.Abstractions;
using Services.Common.Model;
using Xunit;

namespace Services.Applications.Tests;

public class ProductTwoTests
{
    public AdministratorFactory _administratorFactory { get; set; }
    public Mock<IKycService> _mockKycService { get; set; }
    public Mock<IBus> _mockBus { get; set; }
    public new Mock<Services.AdministratorTwo.Abstractions.IAdministrationService> _mockServiceAdministratorTwo { get; set; }
    
    
    public ProductTwoTests()
    {
        _mockServiceAdministratorTwo = new Mock<IAdministrationService>();

        var administratorTwoAdapter = new AdministratorTwoAdapter(_mockServiceAdministratorTwo.Object);

        _administratorFactory = new AdministratorFactory(
            null,
            administratorTwoAdapter
        );

        _mockKycService = new Mock<IKycService>();
        _mockBus = new Mock<IBus>();
    }
    
    [Fact]
    public async Task Application_for_ProductTwo_creates_Investor_Account_and_Processes_Payment_in_AdministratorTwo()
    {
        // Arrange

        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductTwo,
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

        var investorId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        
        var createInvestorResponse = new Result<Guid>(true, null, investorId);
        var createAccountResponse = new Result<Guid>(true, null, accountId);
        var processPaymentResponse = new Result<Guid>(true, null, paymentId);
        
        _mockServiceAdministratorTwo.Setup(s => s.CreateInvestorAsync(It.IsAny<User>()))
            .ReturnsAsync(createInvestorResponse);
        _mockServiceAdministratorTwo.Setup(s => s.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()))
            .ReturnsAsync(createAccountResponse);
        _mockServiceAdministratorTwo.Setup(s => s.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()))
            .ReturnsAsync(processPaymentResponse);

        var processingService = new ProcessingService(_administratorFactory, _mockBus.Object);

        var applicationProcessor = new ApplicationProcessor(
            _mockKycService.Object,
            _mockBus.Object,
            processingService
        );

        // Act
        await applicationProcessor.Process(application);

        // Assert
        _mockServiceAdministratorTwo.Verify(s => s.CreateInvestorAsync(It.IsAny<User>()), Times.Once);
        _mockServiceAdministratorTwo.Verify(s => s.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()), Times.Once);
        _mockServiceAdministratorTwo.Verify(s => s.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()), Times.Once);
        
        _mockBus.Verify(b => b.PublishAsync(It.IsAny<DomainEvent>()), Times.Exactly(3));
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is InvestorCreated)), Times.Once);
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is AccountCreated)), Times.Once);
        _mockBus.Verify(b => b.PublishAsync(It.Is<DomainEvent>(e => e is ApplicationCompleted)), Times.Once);
    }
    
    [Theory]
    [InlineData(17)]
    [InlineData(-1)]
    [InlineData(3)]
    [InlineData(201)]
    [InlineData(999)]
    [InlineData(1002)]
    [InlineData(2002)]
    public async Task Application_for_ProductTwo_Fails_Validation_Age_OutOfRange(int age)
    {
        // Arrange

        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductTwo,
            Applicant = new User
            {
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-age)),
                IsVerified = true
            },
            Payment = new Payment(new BankAccount(), new Money("GBP", 42))
        };

        var userGuid = Guid.NewGuid();
        var kycResult = new Result<KycReport>(true, null, new KycReport(userGuid, true));
        _mockKycService.Setup(k => k.GetKycReportAsync(It.IsAny<User>())).ReturnsAsync(kycResult);

        var investorId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        
        var createInvestorResponse = new Result<Guid>(true, null, investorId);
        var createAccountResponse = new Result<Guid>(true, null, accountId);
        var processPaymentResponse = new Result<Guid>(true, null, paymentId);
        
        _mockServiceAdministratorTwo.Setup(s => s.CreateInvestorAsync(It.IsAny<User>()))
            .ReturnsAsync(createInvestorResponse);
        _mockServiceAdministratorTwo.Setup(s => s.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()))
            .ReturnsAsync(createAccountResponse);
        _mockServiceAdministratorTwo.Setup(s => s.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()))
            .ReturnsAsync(processPaymentResponse);

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
    [InlineData(-1, 0.99)]
    [InlineData(3, 0.99)]
    [InlineData(201, 0.99)]
    [InlineData(999, 0.99)]
    [InlineData(1002, 0.99)]
    [InlineData(2002, 0.99)]
    [InlineData(25, 0.98)]
    [InlineData(25, 0.00)]
    public async Task Application_for_ProductTwo_Fails_Validation(int age, decimal amount)
    {
        // Arrange

        var application = new Common.Model.Application
        {
            ProductCode = ProductCode.ProductTwo,
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

        var investorId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        var createInvestorResponse = new Result<Guid>(true, null, investorId);
        var createAccountResponse = new Result<Guid>(true, null, accountId);
        var processPaymentResponse = new Result<Guid>(true, null, paymentId);

        _mockServiceAdministratorTwo.Setup(s => s.CreateInvestorAsync(It.IsAny<User>()))
            .ReturnsAsync(createInvestorResponse);
        _mockServiceAdministratorTwo.Setup(s => s.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>()))
            .ReturnsAsync(createAccountResponse);
        _mockServiceAdministratorTwo.Setup(s => s.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>()))
            .ReturnsAsync(processPaymentResponse);

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