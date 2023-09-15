namespace Ordering.Application.Dtos.Response.Subscriptions
{
    public class SubscriptionItemDetail
    {
        public string? SubscriptionId { get; set; }
        public string? SubscriptionTypeId { get; set; }
        /// <summary>
        /// json {"name":"duration", "description":"Durasi", "value":"30",  "type":"int"}
        /// </summary>
        public string? SubscriptionValue { get; set; }

    }
}
