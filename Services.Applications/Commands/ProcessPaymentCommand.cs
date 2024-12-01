using MediatR;
using Services.Common.Abstractions.Model;

namespace Services.Applications.Commands;


public record ProcessPaymentCommand(Guid AccountId, Guid ApplicationId, Payment Payment) : IRequest;