using Ordering.Application.EventBus.Events;

namespace Ordering.Application.EventBus.EventHandlings
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
