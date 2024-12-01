using MediatR;

namespace Services.Applications.Commands;

public record SagaCompensationCommand(Guid ApplicationId, string FailedStep, string Reason) : IRequest;
