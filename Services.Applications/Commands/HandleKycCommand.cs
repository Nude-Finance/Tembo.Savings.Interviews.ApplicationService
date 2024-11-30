using MediatR;

namespace Services.Applications.Commands;

public record HandleKycFailureCommand(Guid UserId, Guid ReportId) : IRequest;

