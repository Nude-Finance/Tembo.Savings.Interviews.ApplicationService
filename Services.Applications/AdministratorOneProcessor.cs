namespace Services.Applications;

using AdministratorOne.Abstractions.Model;
using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;

public class AdministratorOneProcessor(
	IBus bus,
	AdministratorOne.Abstractions.IAdministrationService administrationService,
	IProductValidatorFactory productValidatorFactory,
	IKycService kycService)
	: BaseProductProcessor(productValidatorFactory, kycService, bus)
{

	public override HashSet<ProductCode> SupportedProductCodes => [ProductCode.ProductOne];

	protected async override Task ProcessApplicationAsync(Application application)
	{
		var request = MapApplicationToInvestorRequest(application);

		var response = administrationService.CreateInvestor(request);
		
		await Bus.PublishAsync(new InvestorCreated(application.Applicant.Id, response.InvestorId));
	}
	private CreateInvestorRequest MapApplicationToInvestorRequest(Application application)
	{
		return new CreateInvestorRequest
		{
			AccountNumber = application.Applicant.BankAccounts.First().AccountNumber,
			Addressline1 = application.Applicant.Addresses.First().Addressline1,
			Addressline2 = application.Applicant.Addresses.First().Addressline2,
			Addressline3 = application.Applicant.Addresses.First().Addressline3,
			DateOfBirth = application.Applicant.DateOfBirth.ToString(),
			FirstName = application.Applicant.Forename,
			LastName = application.Applicant.Surname,
			InitialPayment = Convert.ToInt32(application.Payment.Amount.Amount * 100),
			Nino = application.Applicant.Nino,

		};
	}
}
