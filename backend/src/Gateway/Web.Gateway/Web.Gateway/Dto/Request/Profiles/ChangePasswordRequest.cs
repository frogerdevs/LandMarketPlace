﻿namespace Web.Gateway.Dto.Request.Profiles
{
    public class ChangePasswordRequest
    {
        public required string Email { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
