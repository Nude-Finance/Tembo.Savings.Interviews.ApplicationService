using MediatR;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Events;

namespace Services.Applications.Commands.Handlers;

public class HandleKycFailureCommandHandler(IMediator mediator, IBus bus) : IRequestHandler<HandleKycFailureCommand>
{
    public async Task Handle(HandleKycFailureCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(
                $"Handling HandleKycFailureCommand: UserId={request.UserId}, ReportId={request.ReportId}");
            await mediator.Publish(
                new ApplicationProcessCompleteCommand(request.ApplicationId, request.ReportId, request.UserId,
                    ApplicationProcessStatus.KycFailure), cancellationToken);
            await bus.PublishAsync(new KycFailed(request.UserId, request.ReportId));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in HandleKycFailureCommandHandler: {ex.Message}");
            await mediator.Send(new SagaCompensationCommand(request.ApplicationId, "HandleKycFailure", ex.Message),
                cancellationToken);
        }
    }
}