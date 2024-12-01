using Services.AdministratorTwo.Abstractions;
using Services.Applications.Abstractions;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Products;

namespace Services.Applications.ApplicationProcessors;

public class ProductTwoApplicationProcessor(
    IApplicationValidator<ProductTwo> applicationValidator,
    IKycService kycService,
    IAdministrationService administratorTwoService)
    : IApplicationProcessor<ProductTwo>
{
    private readonly IApplicationValidator<ProductTwo> _applicationValidator = applicationValidator;
    private readonly IKycService _kycService = kycService;
    private readonly IAdministrationService _administratorTwoService = administratorTwoService;

    public async Task<Result> Process(Application<ProductTwo> application)
    {
        var validationResult = _applicationValidator.Validate(application);
        if (!validationResult.IsSuccess)
        {
            return Result.Failure(validationResult.Error);
        }

        var kycResult = await _kycService.GetKycReportAsync(application.Applicant);
        if (!kycResult.IsSuccess)
        {
            return Result.Failure(kycResult.Error);
        }
        
        var investorResult = await _administratorTwoService.CreateInvestorAsync(application.Applicant);

        if (!investorResult.IsSuccess)
        {
            return Result.Failure(investorResult.Error);
        }
        
        var accountResult = await _administratorTwoService.CreateAccountAsync(investorResult.Value, application.Product.ProductCode);

        if (!accountResult.IsSuccess)
        {
            return Result.Failure(accountResult.Error);
        }
        
        var paymentResult = await _administratorTwoService.ProcessPaymentAsync(accountResult.Value, application.Payment);

        if (!paymentResult.IsSuccess)
        {
            return Result.Failure(paymentResult.Error);
        }
        
        return Result.Success();
    }
}