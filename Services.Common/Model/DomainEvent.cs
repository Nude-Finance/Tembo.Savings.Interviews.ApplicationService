using MediatR;

namespace Services.Common.Abstractions.Model.Events;

public abstract record DomainEvent;

public record InvestorCreated(Guid UserId, string InvestorId) : DomainEvent, INotification;

public record AccountCreated(Guid InvestorId, ProductCode Product, Guid AccountId) : DomainEvent, INotification;

public record KycFailed(Guid UserId, Guid ReportId) : DomainEvent, INotification;

public record ApplicationCompleted(Guid ApplicationId) : DomainEvent, INotification;