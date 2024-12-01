using MediatR;
using Services.AdministratorOne.Abstractions.Model;
using Services.AdministratorTwo.Abstractions;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands.Handlers;

public class CreateAccountCommandHandler(IAdministrationService administrationService, IMediator mediator)
    : IRequestHandler<CreateAccountCommand>
{
    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"Handling CreateAccountCommand: InvestorId={request.InvestorId}, ApplicationId={request.ApplicationId}");
            var accountDetails = administrationService.CreateAccountAsync(request.InvestorId, request.Product).GetAwaiter().GetResult();
            Console.WriteLine($"Account created successfully: AccountId={accountDetails.Value}");
            await mediator.Send(new ProcessPaymentCommand(accountDetails.Value, request.ApplicationId, request.Payment), cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateAccountCommandHandler: {ex.Message}");
            await mediator.Send(new SagaCompensationCommand(request.ApplicationId, "CreateAccount", ex.Message), cancellationToken);
            throw;
        }
    }
}
