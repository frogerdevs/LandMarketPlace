using IdentityServer.Dtos.Requests.Otps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityServer.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    public class OtpController : BaseApiController<UserController>
    {
        private readonly IDistributedCache _cache;
        private readonly Random _random;
        private readonly UserManager<AppUser> _userManager;

        public OtpController(IDistributedCache cache, UserManager<AppUser> userManager)
        {
            _cache = cache;
            _random = new Random();
            _userManager = userManager;
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> SendOtp([FromBody] OtpRequest request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.EmailOrPhone))
            {
                return BadRequest();
            }

            // Check if the user is allowed to request a new OTP code
            if (!IsOtpRequestAllowed(request.EmailOrPhone))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Please wait for 3 minutes before requesting a new OTP code." });
            }

            // Generate random OTP code
            string otpCode = GenerateOtpCode();

            // Set the expiration time (5 minutes from the current time)
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(5);

            // Store the OTP code and its expiration time in the cache
            string cacheKey = $"OTP_{request.EmailOrPhone}";
            var otpData = new
            {
                OtpCode = otpCode,
                ExpirationTime = expirationTime
            };
            string otpDataJson = JsonSerializer.Serialize(otpData);
            await _cache.SetStringAsync(cacheKey, otpDataJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime
            });

            // Store the last request time for the user
            string lastRequestKey = $"LastRequest_{request.EmailOrPhone}";
            string lastRequestTime = DateTime.UtcNow.ToString("O");
            await _cache.SetStringAsync(lastRequestKey, lastRequestTime, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });

            // Send the OTP code to the user via SMS or email
            // ...
            if (ValidateInput.IsValidEmail(request.EmailOrPhone))
            {

            }
            else if (ValidateInput.IsValidPhoneNumber(request.EmailOrPhone))
            {

            }

            return Ok(new BaseResponse { Success = true, Message = $"OTP sent successfully {otpCode}" });
        }
        [HttpPost("[action]")]
        public async ValueTask<ActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            // Validate request
            if (string.IsNullOrEmpty(request.EmailOrPhone))
            {
                return BadRequest(new BaseResponse { Success = false, Message = "Invalid email" });
            }

            // Retrieve the OTP data from the cache
            string cacheKey = $"OTP_{request.EmailOrPhone}";
            string? otpDataJson = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(otpDataJson))
            {
                // OTP data not found in the cache or already expired
                return BadRequest(new BaseResponse { Success = false, Message = "Invalid or expired OTP code" });
            }

            // Deserialize the OTP data from JSON
            var otpData = JsonSerializer.Deserialize<OtpDataModel>(otpDataJson);

            // Compare the provided OTP code with the stored one and check the expiration time
            if (request.OtpCode == otpData?.OtpCode && DateTime.UtcNow <= otpData.ExpirationTime)
            {
                // OTP code is valid and within the expiration time
                // Clear the OTP data from the cache (single-use OTP)
                _cache.Remove(cacheKey);
                if (ValidateInput.IsValidEmail(request.EmailOrPhone))
                {
                    var user = await _userManager.FindByEmailAsync(request.EmailOrPhone);
                    if (user is null)
                        return BadRequest(new BaseResponse { Success = false, Message = "Invalid Email" });

                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                else if (ValidateInput.IsValidPhoneNumber(request.EmailOrPhone))
                {
                    var user = await _userManager.Users.FirstOrDefaultAsync(c => c.PhoneNumber == request.EmailOrPhone);
                    if (user is null)
                        return BadRequest(new BaseResponse { Success = false, Message = "Invalid Phone Number" });
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                return Ok(new BaseResponse { Success = true, Message = "OTP code is valid" });
            }

            // Invalid OTP code or expired
            return BadRequest(new BaseResponse { Success = false, Message = "Invalid or expired OTP code" });
        }
        private bool IsOtpRequestAllowed(string email)
        {
            // Check the last request time for the user
            string lastRequestKey = $"LastRequest_{email}";
            string? lastRequestTime = _cache.GetString(lastRequestKey);

            if (!string.IsNullOrEmpty(lastRequestTime))
            {
                DateTime lastRequest = DateTime.Parse(lastRequestTime);
                if (DateTime.UtcNow.Subtract(lastRequest) < TimeSpan.FromMinutes(3))
                {
                    // If less than 3 minutes have passed since the last request, disallow a new request
                    return false;
                }
            }

            return true;
        }
        private string GenerateOtpCode()
        {
            // Generate a random 6-digit OTP code
            return _random.Next(100000, 999999).ToString();
        }

        public class OtpDataModel
        {
            public string OtpCode { get; set; }
            public DateTime ExpirationTime { get; set; }
        }
    }
}
