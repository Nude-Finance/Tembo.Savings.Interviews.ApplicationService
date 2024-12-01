using MediatR;
using Moq;
using Services.Applications.Strategies.Context;
using Services.Applications.Strategies.InvestorStrategy;
using Services.Applications.Strategies.InvestorStrategy.Validators;
using Services.Common.Abstractions.Model;
using IAdministrationService = Services.AdministratorOne.Abstractions.IAdministrationService;

namespace Services.Applications.Tests.CreateInvestor.Fixtures;
public class StrategyPatternFixture
{
    private Mock<IServiceProvider> MockServiceProvider { get; }
    public Mock<IAdministrationService> MockAdminOneService { get; }
    public Mock<AdministratorTwo.Abstractions.IAdministrationService> MockAdminTwoService { get; }
    public Mock<IMediator> MockMediator { get; }

    public InvestorContext InvestorContext { get; }

    public StrategyPatternFixture()
    {
        MockServiceProvider = new Mock<IServiceProvider>();
        MockAdminOneService = new Mock<IAdministrationService>();
        MockAdminTwoService = new Mock<AdministratorTwo.Abstractions.IAdministrationService>();
        MockMediator = new Mock<IMediator>();

        // ProductOne
        var productOneValidation = new ProductOneInvestorValidation();
        var productOneInvestorCreator = new ProductOneInvestorCreator(productOneValidation, MockAdminOneService.Object);

        // ProductTwo
        var productTwoInvestorCreator = new ProductTwoInvestorCreator(MockAdminTwoService.Object);

        // Setup ServiceProvider
        MockServiceProvider
            .Setup(sp => sp.GetService(typeof(ProductOneInvestorCreator)))
            .Returns(productOneInvestorCreator);

        MockServiceProvider
            .Setup(sp => sp.GetService(typeof(ProductTwoInvestorCreator)))
            .Returns(productTwoInvestorCreator);

        // Initialize InvestorContext
        InvestorContext = new InvestorContext(MockServiceProvider.Object);
    }

    public Application CreateValidApplication(ProductCode productCode)
    {
        return new Application
        {
            Id = Guid.NewGuid(),
            Applicant = new User
            {
                Id = Guid.NewGuid(),
                IsVerified = true,
                ForeName = "John",
                SurName = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Nino = "AB123456C",
                Addresses = new[]
                {
                    new Address
                    {
                        AddressLine1 = "123 Main St",
                        AddressLine2 = "Suite 101",
                        AddressLine3 = "",
                        Country = "UK",
                        PostCode = "AB1 2CD"
                    }
                },
                BankAccounts = new[]
                {
                    new BankAccount
                    {
                        SortCode = "12-34-56",
                        AccountNumber = "12345678"
                    }
                }
            },
            ProductCode = productCode,
            Payment = new Payment(
                new BankAccount
                {
                    SortCode = "12-34-56",
                    AccountNumber = "12345678"
                },
                new Money("GBP", 100m)
            )
        };
    }
}
