using Moq;
using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Strategies.InvestorStrategy.Validators;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Tests.CreateInvestor.Fixtures;

public class ProductOneInvestorCreatorFixture
{
    public ProductOneInvestorValidation Validation { get; }
    public Mock<IAdministrationService> MockAdministrationService { get; }

    // Application scenarios
    public Application ValidApplication { get; }
    public Application ApplicationUnder18 { get; }
    public Application ApplicationInsufficientPayment { get; }

    public ProductOneInvestorCreatorFixture()
    {
        // Instantiate validation
        Validation = new ProductOneInvestorValidation();

        // Mock AdministrationService
        MockAdministrationService = new Mock<IAdministrationService>();
        MockAdministrationService
            .Setup(service => service.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(new CreateInvestorResponse
            {
                InvestorId = Guid.NewGuid().ToString()
            });

        // Valid application
        ValidApplication = new Application
        {
            Id = Guid.NewGuid(),
            Applicant = new User
            {
                Id = Guid.NewGuid(),
                IsVerified = true,
                ForeName = "John",
                SurName = "Doe",
                DateOfBirth = new DateOnly(1990, 1, 1), // Age 33
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
            ProductCode = ProductCode.ProductOne,
            Payment = new Payment(
                new BankAccount
                {
                    SortCode = "12-34-56",
                    AccountNumber = "12345678"
                },
                new Money("GBP", 100m)
            )
        };

        // Application with age < 18
        ApplicationUnder18 = new Application
        {
            Id = Guid.NewGuid(),
            Applicant = new User
            {
                Id = Guid.NewGuid(),
                IsVerified = true,
                ForeName = "Jane",
                SurName = "Smith",
                DateOfBirth = new DateOnly(2010, 1, 1), // Age 13
                Nino = "AB123456C",
                Addresses = new[]
                {
                    new Address
                    {
                        AddressLine1 = "456 Elm St",
                        AddressLine2 = "",
                        AddressLine3 = "",
                        Country = "UK",
                        PostCode = "CD3 4EF"
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
            ProductCode = ProductCode.ProductOne,
            Payment = new Payment(
                new BankAccount
                {
                    SortCode = "12-34-56",
                    AccountNumber = "12345678"
                },
                new Money("GBP", 100m)
            )
        };

        // Application with insufficient payment
        ApplicationInsufficientPayment = new Application
        {
            Id = Guid.NewGuid(),
            Applicant = new User
            {
                Id = Guid.NewGuid(),
                IsVerified = true,
                ForeName = "Emma",
                SurName = "Brown",
                DateOfBirth = new DateOnly(1995, 5, 15), // Age in range
                Nino = "AB123456C",
                Addresses = new[]
                {
                    new Address
                    {
                        AddressLine1 = "789 Pine St",
                        AddressLine2 = "",
                        AddressLine3 = "",
                        Country = "UK",
                        PostCode = "EF5 6GH"
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
            ProductCode = ProductCode.ProductOne,
            Payment = new Payment(
                new BankAccount
                {
                    SortCode = "12-34-56",
                    AccountNumber = "12345678"
                },
                new Money("GBP", 0.50m) // Below min
            )
        };
    }
}
