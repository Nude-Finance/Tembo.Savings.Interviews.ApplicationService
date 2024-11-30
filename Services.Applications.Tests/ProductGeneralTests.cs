namespace Services.Applications.Tests;

using AdministratorOne.Abstractions.Model;
using Common.Abstractions.Model;
using Moq;
using Xunit;

public class ProductGeneralTests : BaseApplicationProcessorTest
{
	[Fact]
	public async Task ProcessApplication_ForProductOne_PublishesEventWhenKycFailed()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductOne);
		KycServiceMock.Setup(p => p.GetKycReportAsync(It.IsAny<User>()))
			.ReturnsAsync(() => Result.Success(new KycReport(Guid.NewGuid(), false)));

		// Act
		await ApplicationProcessor.Process(application);

		// Assert
		BusMock.Verify(
			p => p.PublishAsync(It.IsAny<KycFailed>()),
			Times.Once
		);
	}
	
	
	[Fact]
	public async Task ProcessApplication_ThrowsArgumentExceptionWhenProductIsNotMappedToProcessor()
	{
		// Arrange
		var application = CreateValidApplication((ProductCode) 999); // Invalid product code

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await ApplicationProcessor.Process(application));

	}
	
	[Fact]
	public async Task ProcessApplication_ThrowsErrorWhenKycReportCallIsNotSuccessful()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductOne);
		KycServiceMock.Setup(p => p.GetKycReportAsync(It.IsAny<User>()))
			.ReturnsAsync(() => Result.Failure<KycReport>(new Error("System", "ERR01", "KYC report failed")));

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(async () => await ApplicationProcessor.Process(application));
	}

	[Fact]
	public async Task ProcessApplication_ShouldNotRunKycReport_ShouldProcessProductWhenApplicationIsVerified()
	{
		//Arrange
		var application = CreateValidApplication(ProductCode.ProductOne, isVerified: true);
		
		//Act
		await ApplicationProcessor.Process(application);
		
		//Assert
		AdministrationOneServiceMock.Verify(
			p => p.CreateInvestor(It.IsAny<CreateInvestorRequest>()),
			Times.Once
		);
		
		KycServiceMock.Verify(p=>p.GetKycReportAsync(It.IsAny<User>()), Times.Never);
	}
	
	[Fact]
	public async Task ProcessApplication_ShouldNotRunKycReport_ShouldNotProcessProductWhenApplicationIsNotVerified()
	{
		//Arrange
		var application = CreateValidApplication(ProductCode.ProductOne, isVerified: false);
		
		//Act
		await ApplicationProcessor.Process(application);
		
		//Assert
		AdministrationOneServiceMock.Verify(
			p => p.CreateInvestor(It.IsAny<CreateInvestorRequest>()),
			Times.Never
		);
		
		KycServiceMock.Verify(p=>p.GetKycReportAsync(It.IsAny<User>()), Times.Never);
	}
}
