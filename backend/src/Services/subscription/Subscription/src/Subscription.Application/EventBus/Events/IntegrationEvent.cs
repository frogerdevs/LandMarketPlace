using Subscription.Application.Enums;

namespace Subscription.Application.EventBus.Events
{
    public record IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
        }

        //[JsonConstructor]
        //public IntegrationEvent(Guid id, DateTime createDate)
        //{
        //    Id = id;
        //    CreationDate = createDate;
        //}
        [JsonInclude]
        public Guid Id { get; private init; }

        //[JsonInclude]
        //public DateTime CreationDate { get; private init; }
        public string? Tenant { get; set; }
        public ActionType Action { get; set; }
    }
}
