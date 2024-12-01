using MediatR;
using IAdministrationService = Services.AdministratorTwo.Abstractions.IAdministrationService;

namespace Services.Applications.Commands.Handlers;

public class ProcessPaymentCommandHandler(IAdministrationService paymentService, IMediator mediator)
    : IRequestHandler<ProcessPaymentCommand>
{
    public async Task Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(
                $"Handling ProcessPaymentCommand: AccountId={request.AccountId}, ApplicationId={request.ApplicationId}");
            var paymentResult = await paymentService.ProcessPaymentAsync(request.AccountId, request.Payment);
            if (!paymentResult.IsSuccess)
            {
                throw new InvalidOperationException("Payment processing failed.");
            }

            Console.WriteLine($"Payment processed successfully for ApplicationId={request.ApplicationId}");

            await mediator.Send(new ApplicationProcessCompleteCommand(
                request.ApplicationId, null, null,
                ApplicationProcessStatus.Success), cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ProcessPaymentCommandHandler: {ex.Message}");
            await mediator.Send(new SagaCompensationCommand(request.ApplicationId, "ProcessPayment", ex.Message),
                cancellationToken);
            throw;
        }
    }
}