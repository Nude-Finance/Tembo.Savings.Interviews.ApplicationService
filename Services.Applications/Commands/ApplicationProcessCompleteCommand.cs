using MediatR;

namespace Services.Applications.Commands;

public record ApplicationProcessCompleteCommand(
    Guid ApplicationId,
    Guid? UserId,
    Guid? ReportId,
    ApplicationProcessStatus Status) : IRequest;

public enum ApplicationProcessStatus
{
    Success,
    ValidationFailed,
    KycFailure
}
