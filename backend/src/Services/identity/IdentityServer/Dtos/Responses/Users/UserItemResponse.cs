﻿namespace IdentityServer.Dtos.Responses.Users
{
    public class UserItemResponse
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool Active { get; set; }
        public bool IsCompany { get; set; }
        public string? BrandName { get; set; }
        public string? BrandSlug { get; set; }
    }
}
