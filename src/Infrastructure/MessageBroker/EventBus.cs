using Application.Abstractions.EventBus;
using MassTransit;
using MassTransit.Testing;

namespace Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class =>
        _publishEndpoint.Publish(@event, cancellationToken);
}
