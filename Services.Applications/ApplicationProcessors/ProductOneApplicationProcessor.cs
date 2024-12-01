using Services.AdministratorOne.Abstractions;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Abstractions;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications.ApplicationProcessors;

public class ProductOneApplicationProcessor(
    IApplicationValidator<ProductOne> applicationValidator,
    IKycService kycService,
    IAdministrationService administratorOneService)
    : IApplicationProcessor<ProductOne>
{
    private readonly IAdministrationService _administratorOneService = administratorOneService;
    private readonly IApplicationValidator<ProductOne> _applicationValidator = applicationValidator; 

    public async Task<Result> Process(Application<ProductOne> application)
    {
        var validationResult = _applicationValidator.Validate(application);

        if (!validationResult.IsSuccess)
            return validationResult;

        var kycResult = await kycService.GetKycReportAsync(application.Applicant);
        if (!kycResult.IsSuccess)
            return kycResult;
        
        var createInvestorRequest = new CreateInvestorRequest
        {
            Addressline1 = application.Applicant.Addresses.First().Addressline1,
            Addressline2 = application.Applicant.Addresses.First().Addressline2,
            Addressline3 = application.Applicant.Addresses.First().Addressline3,
            PostCode = application.Applicant.Addresses.First().PostCode,
            FirstName = application.Applicant.Forename,
            LastName = application.Applicant.Surname,
            AccountNumber = application.Payment.BankAccount.AccountNumber,
            Nino = application.Applicant.Nino,
            //InitialPayment = application.Payment.Amount.Amount,
            //Reference = application.Id.ToString(),
            //DateOfBirth = DateOfBirth,
            //Email = null
        };
        
        var createInvestorResponse = _administratorOneService.CreateInvestor(createInvestorRequest);
        
        return Result.Success(createInvestorResponse);
    }
}