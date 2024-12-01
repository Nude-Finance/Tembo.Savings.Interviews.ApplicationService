using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Strategies.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Strategies.InvestorStrategy;

public class ProductOneInvestorCreator(
    IInvestorValidation validation,
    IAdministrationService administrationService)
    : IInvestorCreator
{
    public Task<Guid> CreateInvestorAsync(Application application)
    {
        validation.Validate(application);
        var request = MapToCreateInvestorRequest(application);
        var response = administrationService.CreateInvestor(request);
        if (response == null || string.IsNullOrEmpty(response.InvestorId))
        {
            throw new InvalidOperationException("Failed to create investor for ProductOne.");
        }
        var id = Guid.Parse(response.InvestorId);
        return Task.FromResult(id);
    }

    private static CreateInvestorRequest MapToCreateInvestorRequest(Application application)
    {
        var applicant = application.Applicant;
        var address = applicant.Addresses.FirstOrDefault();

        return new CreateInvestorRequest
        {
            Reference = application.Id.ToString(),
            FirstName = applicant.ForeName,
            LastName = applicant.SurName,
            DateOfBirth = applicant.DateOfBirth.ToString("yyyy-MM-dd"),
            Nino = applicant.Nino,
            Addressline1 = address?.AddressLine1 ?? string.Empty,
            Addressline2 = address?.AddressLine2 ?? string.Empty,
            Addressline3 = address?.AddressLine3 ?? string.Empty,
            Addressline4 = string.Empty,
            PostCode = address?.PostCode ?? string.Empty,
            Email = string.Empty,
            MobileNumber = string.Empty,
            Product = application.ProductCode.ToString(),
            SortCode = application.Payment.BankAccount.SortCode,
            AccountNumber = application.Payment.BankAccount.AccountNumber,
            InitialPayment = (int)application.Payment.Amount.Amount
        };
    }
}