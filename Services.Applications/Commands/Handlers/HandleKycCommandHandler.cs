using MediatR;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands.Handlers;

public record HandleKycCommand(Application Application) : IRequest;
