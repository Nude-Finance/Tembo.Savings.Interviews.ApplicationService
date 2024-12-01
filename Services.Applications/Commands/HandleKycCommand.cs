using MediatR;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands;

public record HandleKycCommand(Application Application) : IRequest;


