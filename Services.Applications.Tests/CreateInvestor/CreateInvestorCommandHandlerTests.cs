using Moq;
using Services.AdministratorOne.Abstractions.Model;
using Services.Applications.Commands;
using Services.Applications.Commands.Handlers;
using Services.Applications.Tests.CreateInvestor.Fixtures;
using Services.Common.Abstractions.Model;
using Xunit;

namespace Services.Applications.Tests.CreateInvestor;
public class CreateInvestorCommandHandlerTests(StrategyPatternFixture fixture) : IClassFixture<StrategyPatternFixture>
{
    [Fact]
    public async Task Handle_ProductOne_TransitionsToApplicationProcessCompleteCommand()
    {
        // Arrange
        var application = fixture.CreateValidApplication(ProductCode.ProductOne);
        var investorId = Guid.NewGuid();

        var command = new CreateInvestorCommand(application);

        fixture.MockAdminOneService
            .Setup(service => service.CreateInvestor(It.IsAny<CreateInvestorRequest>()))
            .Returns(new CreateInvestorResponse
            {
                InvestorId = investorId.ToString(),
                AccountId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString()
            });

        var handler = new CreateInvestorCommandHandler(fixture.InvestorContext, fixture.MockMediator.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        fixture.MockMediator.Verify(mediator => mediator.Send(It.Is<ApplicationProcessCompleteCommand>(c =>
            c.ApplicationId == application.Id &&
            c.UserId == application.Applicant.Id &&
            c.Status == ApplicationProcessStatus.Success), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductTwo_NavigateToCreateAccountCommand()
    {
        // Arrange
        var application = fixture.CreateValidApplication(ProductCode.ProductTwo);
        var investorId = Guid.NewGuid();

        var command = new CreateInvestorCommand(application);

        fixture.MockAdminTwoService
            .Setup(service => service.CreateInvestorAsync(application.Applicant))
            .ReturnsAsync(Result.Success(investorId));

        var handler = new CreateInvestorCommandHandler(fixture.InvestorContext, fixture.MockMediator.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        fixture.MockMediator.Verify(mediator => mediator.Send(It.Is<CreateAccountCommand>(c =>
            c.ApplicationId == application.Id &&
            c.Product == ProductCode.ProductTwo &&
            c.InvestorId == investorId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Exception_TriggersSagaCompensationCommand()
    {
        // Arrange
        var application = fixture.CreateValidApplication(ProductCode.ProductTwo);

        var command = new CreateInvestorCommand(application);

        fixture.MockAdminTwoService
            .Setup(service => service.CreateInvestorAsync(application.Applicant))
            .ThrowsAsync(new Exception("Test error")); //Throw an unexpected error

        var handler = new CreateInvestorCommandHandler(fixture.InvestorContext, fixture.MockMediator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

        fixture.MockMediator.Verify(mediator => mediator.Send(It.Is<SagaCompensationCommand>(c =>
            c.ApplicationId == application.Id &&
            c.FailedStep == "CreateInvestor" &&
            c.Reason == "Test error"), It.IsAny<CancellationToken>()), Times.Once); //Handle the rollback navigation
    }
}

