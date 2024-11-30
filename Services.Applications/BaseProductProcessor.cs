namespace Services.Applications;

using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;

public abstract class BaseProductProcessor(IProductValidatorFactory productValidatorFactory, IKycService kycService, IBus bus) : IProductProcessor
{
	public abstract HashSet<ProductCode> SupportedProductCodes { get; }

	protected readonly IBus Bus = bus;

	public async Task ProcessAsync(Application application)
	{
		ValidateProductCode(application.ProductCode);

		var validator = productValidatorFactory.GetValidator(application.ProductCode);
		if (validator != null &&
		    !validator.Validate(application))
		{
			return;
		}

		if (!await ValidateKycStatusAsync(application.Applicant))
		{
			return;
		}

		await ProcessApplicationAsync(application);
	}

	protected abstract Task ProcessApplicationAsync(Application application);

	private void ValidateProductCode(ProductCode productCode)
	{
		if (!SupportedProductCodes.Contains(productCode))
		{
			throw new ArgumentException($"Unsupported ProductCode: {productCode}");
		}
	}

	private async Task<bool> ValidateKycStatusAsync(User applicant)
	{
		if (applicant.IsVerified.HasValue)
		{
			return applicant.IsVerified.Value;
		}

		var kycReport = await FetchKycReportAsync(applicant);
		if (kycReport.IsVerified)
		{
			return true;
		}
		
		await HandleKycFailureAsync(applicant, kycReport.Id);
		return false;
	}

	private async Task<KycReport> FetchKycReportAsync(User applicant)
	{
		var kycReport = await kycService.GetKycReportAsync(applicant);
		if (!kycReport.IsSuccess)
		{
			throw new InvalidOperationException("KYC report error.");
		}

		return kycReport.Value;
	}

	private async Task HandleKycFailureAsync(User applicant, Guid reportId)
	{
		await Bus.PublishAsync(new KycFailed(applicant.Id, reportId));
	}

}
