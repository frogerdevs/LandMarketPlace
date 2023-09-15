using Catalog.Application.EventBus.Events;

namespace Catalog.Application.EventBus.EventHandlings
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
