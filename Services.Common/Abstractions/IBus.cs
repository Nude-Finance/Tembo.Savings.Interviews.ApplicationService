using Services.Common.Model;

namespace Services.Common.Abstractions;

public interface IBus
{
    Task PublishAsync(DomainEvent domainEvent);
}