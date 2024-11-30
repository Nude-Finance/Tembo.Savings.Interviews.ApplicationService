namespace Services.Applications;

using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;

public class AdministratorTwoProcessor(
	IBus bus,
	AdministratorTwo.Abstractions.IAdministrationService administrationService,
	IProductValidatorFactory productValidatorFactory,
	IKycService kycService) : BaseProductProcessor(productValidatorFactory, kycService, bus)
{
	public override HashSet<ProductCode> SupportedProductCodes => [ProductCode.ProductTwo];

	protected async override Task ProcessApplicationAsync(Application application)
	{
		var createInvestorResponse = await administrationService.CreateInvestorAsync(application.Applicant);
		if (!createInvestorResponse.IsSuccess)
		{
			throw new InvalidOperationException($"Failed to create investor for ApplicantId: {application.Applicant.Id}.");
		}

		await Bus.PublishAsync(new InvestorCreated(application.Applicant.Id, createInvestorResponse.Value.ToString()));

		var createAccountResponse = await administrationService.CreateAccountAsync(createInvestorResponse.Value, application.ProductCode);
		if (!createAccountResponse.IsSuccess)
		{
			// Rollback logic can be added here
			throw new InvalidOperationException($"Failed to create account for InvestorId: {createAccountResponse.Value}.");
		}

		await Bus.PublishAsync(new AccountCreated(createInvestorResponse.Value.ToString(), application.ProductCode, createAccountResponse.Value.ToString()));

		var processPaymentResult = await administrationService.ProcessPaymentAsync(createAccountResponse.Value, application.Payment);
		if (!processPaymentResult.IsSuccess)
		{
			// Rollback logic can be added here
			throw new InvalidOperationException($"Failed to process payment for AccountId: {processPaymentResult.Value}");
		}

		await Bus.PublishAsync(new ApplicationCompleted(processPaymentResult.Value));
	}
}
