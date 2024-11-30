namespace Services.Applications.Tests;

using AdministratorOne.Abstractions.Model;
using Common.Abstractions.Abstractions;
using Common.Abstractions.Model;
using Common.Abstractions.Validators;
using Moq;

public abstract class BaseApplicationProcessorTest : IDisposable
{
	protected readonly Mock<IKycService> KycServiceMock;
	protected readonly Mock<IBus> BusMock;
	protected readonly Mock<AdministratorOne.Abstractions.IAdministrationService> AdministrationOneServiceMock;
	protected readonly Mock<AdministratorTwo.Abstractions.IAdministrationService> AdministrationTwoServiceMock;
	protected readonly ApplicationProcessor ApplicationProcessor;
	protected readonly Guid InvestorId = Guid.NewGuid();
	protected readonly Guid AccountId = Guid.NewGuid();
	protected readonly Guid PaymentId = Guid.NewGuid();

	protected BaseApplicationProcessorTest()
	{
		KycServiceMock = CreateKycServiceMock();
		BusMock = new Mock<IBus>();
		AdministrationOneServiceMock = CreateAdministrationOneServiceMock();
		AdministrationTwoServiceMock = CreateAdministrationTwoServiceMock();

		var productProcessorFactory = SetupProductProcessing();

		ApplicationProcessor = new ApplicationProcessor(productProcessorFactory);
	}

	private ProductProcessorFactory SetupProductProcessing()
	{
		var ageCalculator = new AgeCalculator();
		var ageValidator = new AgeValidator(ageCalculator);
		var moneyValidator = new MoneyValidator();

		var productValidators = new IProductValidator[]
		{
			new ProductOneValidator(ageValidator, moneyValidator),
			new ProductTwoValidator(ageValidator, moneyValidator)
		};

		var productValidatorFactory = new ProductValidatorFactory(productValidators);

		var productOneProcessor = new AdministratorOneProcessor(
			BusMock.Object,
			AdministrationOneServiceMock.Object,
			productValidatorFactory,
			KycServiceMock.Object
		);

		var productTwoProcessor = new AdministratorTwoProcessor(
			BusMock.Object,
			AdministrationTwoServiceMock.Object,
			productValidatorFactory,
			KycServiceMock.Object
		);

		var productProcessorFactory = new ProductProcessorFactory(
			new BaseProductProcessor[] {productOneProcessor, productTwoProcessor}
		);

		return productProcessorFactory;
	}

	private Mock<IKycService> CreateKycServiceMock()
	{
		var mock = new Mock<IKycService>();
		mock.Setup(p => p.GetKycReportAsync(It.IsAny<User>()))
			.ReturnsAsync(() => Result.Success(new KycReport(Guid.NewGuid(), true)));
		return mock;
	}

	private Mock<AdministratorOne.Abstractions.IAdministrationService> CreateAdministrationOneServiceMock()
	{
		var mock = new Mock<AdministratorOne.Abstractions.IAdministrationService>();
		mock.Setup(p => p.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
			.Returns(() => new CreateInvestorResponse
			{
				InvestorId = Guid.NewGuid().ToString()
			});
		return mock;
	}

	private Mock<AdministratorTwo.Abstractions.IAdministrationService> CreateAdministrationTwoServiceMock()
	{
		var mock = new Mock<AdministratorTwo.Abstractions.IAdministrationService>();
		mock.Setup(p => p.CreateInvestorAsync(It.IsAny<User>())).ReturnsAsync(Result.Success(InvestorId));
		mock.Setup(p => p.CreateAccountAsync(It.IsAny<Guid>(), It.IsAny<ProductCode>())).ReturnsAsync(() => Result.Success(AccountId));
		mock.Setup(p => p.ProcessPaymentAsync(It.IsAny<Guid>(), It.IsAny<Payment>())).ReturnsAsync(() => Result.Success(PaymentId));

		return mock;
	}

	protected Application CreateValidApplication(ProductCode productCode, int age = 25, bool? isVerified =null)
	{
		return new Application
		{
			Applicant = new User
			{
				Addresses =
				[
					new Address
					{
						Addressline1 = "address line 1",
						Addressline2 = "address line 2",
						Addressline3 = "address line 3",
						Country = "United Kingdom",
						PostCode = "SW19 1RT"
					}
				],
				BankAccounts =
				[
					new BankAccount
					{
						AccountNumber = "123456",
						SortCode = "11-22-33"
					}
				],
				DateOfBirth = new DateOnly(DateTime.Today.AddYears(-age).Year, 1, 24),
				Id = Guid.NewGuid(),
				Forename = "John",
				Nino = "AVF112233",
				Surname = "Doe",
				IsVerified = isVerified
			},
			Id = Guid.NewGuid(),
			Payment = new Payment(
				new BankAccount
				{
					AccountNumber = "123456789",
					SortCode = "44-44-44"
				},
				new Money("USD", 1000)
			),
			ProductCode = productCode
		};
	}

	public virtual void Dispose()
	{
		GC.SuppressFinalize(this);
	}
}
