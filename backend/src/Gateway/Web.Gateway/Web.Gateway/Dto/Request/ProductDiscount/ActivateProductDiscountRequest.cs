namespace Web.Gateway.Dto.Request.ProductDiscount
{
    public class ActivateProductDiscountRequest
    {
        public required string Id { get; set; }
        public bool Active { get; set; }

    }
}
