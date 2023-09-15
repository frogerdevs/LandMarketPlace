namespace Subscription.Application.Dtos.Response.UnitTypes
{
    public class UnitTypeItem
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public bool Active { get; set; }

    }
}
