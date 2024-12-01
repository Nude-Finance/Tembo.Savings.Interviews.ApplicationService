namespace Services.Applications.Tests.CreateInvestor.Fixtures;

using Moq;
using MediatR;
using Services.Applications.Commands.Handlers;
using Services.Applications.Strategies.Context;
using Services.Common.Abstractions.Model;

public class CreateInvestorCommandHandlerFixture
{
    public Mock<InvestorContext> MockInvestorContext { get; }
    public Mock<IMediator> MockMediator { get; }
    public CreateInvestorCommandHandler Handler { get; }

    public CreateInvestorCommandHandlerFixture()
    {
        MockInvestorContext = new Mock<InvestorContext>();
        MockMediator = new Mock<IMediator>();

        Handler = new CreateInvestorCommandHandler(MockInvestorContext.Object, MockMediator.Object);
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
