﻿namespace Web.Gateway.Dto.Response.Category
{
    public class CategoryItem
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool? Active { get; set; }
    }
}
