using MediatR;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands;

public record CreateInvestorCommand(Application Application) : IRequest;