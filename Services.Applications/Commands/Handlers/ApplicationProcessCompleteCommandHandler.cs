using MediatR;
using Services.Common.Abstractions.Abstractions;
using Services.Common.Abstractions.Model;
using Services.Common.Abstractions.Model.Events;

namespace Services.Applications.Commands.Handlers;

public class ApplicationProcessCompleteCommandHandler(IBus bus) : IRequestHandler<ApplicationProcessCompleteCommand>
{
    public async Task Handle(ApplicationProcessCompleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(
                $"Handling ApplicationProcessCompleteCommand: ApplicationId={request.ApplicationId}, Status={request.Status}");

            switch (request)
            {
                case { Status: ApplicationProcessStatus.Success }:
                    await HandleSuccessAsync(request.ApplicationId, cancellationToken);
                    break;
                case { Status: ApplicationProcessStatus.ValidationFailed }:
                    await HandleValidationfailedAsync(request.ApplicationId, cancellationToken);
                    break;
                case { Status: ApplicationProcessStatus.KycFailure }:
                    await HandleKycFailureAsync(request.ApplicationId, request.UserId, request.ReportId);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown application process status: {request.Status}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ApplicationProcessCompleteCommandHandler: {ex.Message}");
            throw;
        }
    }

    private async Task HandleSuccessAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Application successfully completed: ApplicationId={applicationId}");
        await bus.PublishAsync(new ApplicationCompleted(applicationId));
    }
    private async Task HandleValidationfailedAsync(Guid applicationId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Validation failed for: ApplicationId={applicationId}");
        await bus.PublishAsync(new ApplicationCompleted(applicationId));
    }

    private async Task HandleKycFailureAsync(Guid applicationId, Guid? userId, Guid? reportId)
    {
        Console.WriteLine($"KYC failed for ApplicationId={applicationId}: UserId: {userId}, ReportId={reportId}");
        await bus.PublishAsync(new KycFailed(userId!.Value, reportId!.Value));
    }
}