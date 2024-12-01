using Moq;
using Xunit;

namespace Services.Applications.Tests;

using AdministratorOne.Abstractions.Model;
using Common.Abstractions.Model;

public class ProductOneProcessingTests : BaseApplicationProcessorTest
{
	[Fact]
	public async Task ProcessApplication_ForProductOne_CreatesInvestorInAdministratorOne()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductOne);

		// Act
		await ApplicationProcessor.Process(application);

		// Assert
		AdministrationOneServiceMock.Verify(
			p => p.CreateInvestor(It.IsAny<CreateInvestorRequest>()),
			Times.Once
		);

		AdministrationTwoServiceMock.Verify(
			p => p.CreateInvestorAsync(It.IsAny<User>()),
			Times.Never
		);
	}

}
