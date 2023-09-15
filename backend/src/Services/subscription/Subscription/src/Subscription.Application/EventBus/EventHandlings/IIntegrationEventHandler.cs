using Subscription.Application.EventBus.Events;

namespace Subscription.Application.EventBus.EventHandlings
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
     where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
