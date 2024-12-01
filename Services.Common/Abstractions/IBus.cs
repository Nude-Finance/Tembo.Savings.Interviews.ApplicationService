using Services.Common.Abstractions.Model.Events;

namespace Services.Common.Abstractions.Abstractions;

public interface IBus
{
    Task PublishAsync(DomainEvent domainEvent);
}