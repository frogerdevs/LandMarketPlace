using IdentityServer.Data.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Identity.Api.Test.Controllers.Api.v1
{
    [TestFixture]
    public class OtpControllerTests
    {
        private OtpController _otpController;
        private Mock<IDistributedCache> _mockCache;
        private Mock<UserManager<AppUser>> _mockUserManager;

        [SetUp]
        public void SetUp()
        {
            _mockCache = new Mock<IDistributedCache>();

            var users = new List<AppUser>
            {
                new AppUser { UserName = "user1", Email = "user1@example.com", PhoneNumber = "1234567890" },
                new AppUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "9876543210" }
            };

            var mockUserStore = Mock.Of<IUserStore<AppUser>>();
            var mockUserQueryable = users.BuildMock();
            _mockUserManager = new Mock<UserManager<AppUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            _mockUserManager.Setup(u => u.Users).Returns(mockUserQueryable);
            _mockUserManager.Setup(userManager => userManager.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                            .Returns(Task.FromResult(IdentityResult.Success));

            _otpController = new OtpController(_mockCache.Object, _mockUserManager.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = new ServiceCollection()
                        .AddScoped(_ => _mockCache.Object)
                        .AddScoped(_ => _mockUserManager.Object)
                        .BuildServiceProvider()
                    }
                }
            };
        }

        [Test]
        public async Task SendOtp_ValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var request = new OtpRequest { EmailOrPhone = "test@example.com" };
            //_mockCache.Setup(c => c.GetString("LastRequest_test@example.com")).Returns((string)null);
            _mockCache
               .Setup(cache => cache.Get(It.IsAny<string>()))
               .Returns(() =>
               {
                   // Simulate retrieving data from cache
                   return null;
               });

            // Mock a random number generator
            var randomMock = new Mock<Random>();
            randomMock.Setup(r => r.Next(It.IsAny<int>(), It.IsAny<int>()))
                      .Returns(123456); // Use a specific OTP code for testing


            // Act
            var result = await _otpController.SendOtp(request) as OkObjectResult;
            var dt = (BaseResponse)result!.Value!;
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(dt.Message, Is.EqualTo("OTP sent successfully"));
            });
        }

        [Test]
        public async Task SendOtp_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OtpRequest(); // Invalid request without the required 'Email' property

            // Act
            var result = await _otpController.SendOtp(request) as BadRequestObjectResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result!.Value!, Is.InstanceOf<SerializableError>());
            });
        }

        [Test]
        public async Task SendOtp_RequestNotAllowed_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OtpRequest { EmailOrPhone = "test@example.com" };
            var lastRequestTime = DateTime.UtcNow.AddMinutes(-2).ToString("O");

            // Mock cache methods
            string expectedCacheKey = $"OTP_{request.EmailOrPhone}";
            string lastRequestKey = $"LastRequest_{request.EmailOrPhone}";

            DateTime expectedExpirationTime = DateTime.UtcNow.AddMinutes(5);
            _mockCache
               .Setup(cache => cache.Get(It.IsAny<string>()))
               .Returns(() =>
               {
                   // Simulate retrieving data from cache
                   return Encoding.UTF8.GetBytes(lastRequestTime);
               });

            // Act
            var result = await _otpController.SendOtp(request) as BadRequestObjectResult;
            var dt = (BaseResponse)result!.Value!;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(dt.Message, Is.EqualTo("Please wait for 3 minutes before requesting a new OTP code."));
            });
        }

        [Test]
        public async Task VerifyOtp_ValidOtpCode_ShouldReturnOkResult()
        {
            // Arrange
            var request = new VerifyOtpRequest { EmailOrPhone = "test@example.com", OtpCode = "123456" };
            var otpData = new { OtpCode = "123456", ExpirationTime = DateTime.UtcNow.AddMinutes(1) };
            _mockCache.Setup(c => c.GetAsync("OTP_test@example.com", default)).ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(otpData)));

            // Act
            var result = await _otpController.VerifyOtp(request) as OkObjectResult;
            var dt = (BaseResponse)result!.Value!;
            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(dt.Message, Is.EqualTo("OTP code is valid"));
            });
            _mockCache.Verify(c => c.Remove("OTP_test@example.com"), Times.Once);
        }

        [Test]
        public async Task VerifyOtp_ExpiredOtpCode_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new VerifyOtpRequest { EmailOrPhone = "test@example.com", OtpCode = "123456" };
            var otpData = new { OtpCode = "123456", ExpirationTime = DateTime.UtcNow.AddMinutes(-1) };
            _mockCache.Setup(c => c.GetAsync("OTP_test@example.com", default)).ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(otpData)));

            // Act
            var result = await _otpController.VerifyOtp(request) as BadRequestObjectResult;
            var dt = (BaseResponse)result!.Value!;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(dt.Message, Is.EqualTo("Invalid or expired OTP code"));
            });
            _mockCache.Verify(c => c.Remove("OTP_test@example.com"), Times.Never);
        }

        [Test]
        public async Task VerifyOtp_InvalidOtpCode_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new VerifyOtpRequest { EmailOrPhone = "test@example.com", OtpCode = "654321" };
            var otpData = new { OtpCode = "123456", ExpirationTime = DateTime.UtcNow.AddMinutes(1) };
            _mockCache.Setup(c => c.GetAsync("OTP_test@example.com", default)).ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(otpData)));

            // Act
            var result = await _otpController.VerifyOtp(request) as BadRequestObjectResult;
            var dt = (BaseResponse)result!.Value!;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(dt.Message, Is.EqualTo("Invalid or expired OTP code"));
            });
            _mockCache.Verify(c => c.Remove("OTP_test@example.com"), Times.Never);
        }
    }
}