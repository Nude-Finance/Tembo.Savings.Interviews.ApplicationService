namespace Services.Applications.Tests;

using AdministratorOne.Abstractions.Model;
using Common.Abstractions.Model;
using Moq;
using Xunit;

public class ProductTwoProcessingTests : BaseApplicationProcessorTest
{
	[Fact]
	public async Task ProcessApplication_ForProductTwo_CompletesProcessInAdministratorTwo()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductTwo);

		// Act
		await ApplicationProcessor.Process(application);

		// Assert
		AdministrationOneServiceMock.Verify(
			p => p.CreateInvestor(It.IsAny<CreateInvestorRequest>()),
			Times.Never
		);

		AdministrationTwoServiceMock.Verify(
			p => p.CreateInvestorAsync(It.IsAny<User>()),
			Times.Once
		);
		
		AdministrationTwoServiceMock.Verify(
			p => p.CreateAccountAsync(InvestorId,ProductCode.ProductTwo),
			Times.Once
		);
		
		AdministrationTwoServiceMock.Verify(
			p => p.ProcessPaymentAsync(AccountId,It.IsAny<Payment>()),
			Times.Once
		);
		
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<InvestorCreated>()), Times.Once);
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<AccountCreated>()), Times.Once);
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<ApplicationCompleted>()), Times.Once);
	}

	[Fact]
	public async Task ProcessApplication_ForProductTwo_ThrowsExceptionWhenCreateInvestorAsyncReturnsNotSuccessful()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductTwo);
		AdministrationTwoServiceMock.Setup(p => p.CreateInvestorAsync(It.IsAny<User>()))
			.ReturnsAsync(Result.Failure<Guid>(new Error("System", "ERR02", "Create investor failed")));

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(async () => await ApplicationProcessor.Process(application));
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<InvestorCreated>()), Times.Never);
		
	}
	
	[Fact]
	public async Task ProcessApplication_ForProductTwo_ThrowsExceptionWhenCreateAccountAsyncReturnsNotSuccessful()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductTwo);
		AdministrationTwoServiceMock.Setup(p => p.CreateAccountAsync(It.IsAny<Guid>(),ProductCode.ProductTwo))
			.ReturnsAsync(Result.Failure<Guid>(new Error("System", "ERR02", "Create investor failed")));

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(async () => await ApplicationProcessor.Process(application));
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<AccountCreated>()), Times.Never);
	}
	
	[Fact]
	public async Task ProcessApplication_ForProductTwo_ThrowsExceptionWhenProcessPaymentAsyncReturnsNotSuccessful()
	{
		// Arrange
		var application = CreateValidApplication(ProductCode.ProductTwo);
		AdministrationTwoServiceMock.Setup(p => p.ProcessPaymentAsync(It.IsAny<Guid>(),It.IsAny<Payment>()))
			.ReturnsAsync(Result.Failure<Guid>(new Error("System", "ERR02", "Create investor failed")));

		// Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(async () => await ApplicationProcessor.Process(application));
		BusMock.Verify(p=>p.PublishAsync(It.IsAny<ApplicationCompleted>()), Times.Never);
	}
}
