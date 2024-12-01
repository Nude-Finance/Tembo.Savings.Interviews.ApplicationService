using MediatR;

namespace Services.Applications.Commands;
public record HandleKycFailureCommand(Guid ApplicationId, Guid UserId, Guid ReportId) : IRequest;