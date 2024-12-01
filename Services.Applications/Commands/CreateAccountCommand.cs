using MediatR;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands;

public record CreateAccountCommand(Guid InvestorId, ProductCode Product, Guid ApplicationId, Payment Payment) : IRequest;


