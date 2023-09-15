﻿namespace Web.Gateway.Dto.Response.Product
{
    public class HomeProductItem
    {
        public required string Id { get; set; }

        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal OldUnitPrice { get; set; }

        public int Quantity { get; set; }

        public string? PictureUrl { get; set; }
    }
}
