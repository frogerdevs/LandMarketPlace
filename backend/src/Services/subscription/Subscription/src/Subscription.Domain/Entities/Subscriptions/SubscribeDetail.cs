namespace Ordering.Domain.Entities.Subscriptions
{
    public class SubscribeDetail : BaseEntity<string>
    {
        public string? SubscribeId { get; set; }
        public string? SubscribeTypeId { get; set; }
        /// <summary>
        /// json {"name":"duration", "description":"Durasi", "value":"30",  "type":"int"}
        /// </summary>
        public string? SubscribeValue { get; set; }
        public Subscribe? Subscribe { get; set; }
        //public SubscriptionType? SubscriptionType { get; set; }
        public SubscribeDetail()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
