using IdentityServer.Data.Entites;
using IdentityServer.Features.Profile.Commands;
using IdentityServer.Features.Users.Queries;
using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Test.Controllers.Api.v1
{
    [TestFixture()]
    public class ProfileControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<ProfileController>> _loggerMock;
        private ProfileController _profileController;

        private Mock<UserManager<AppUser>> _userManagerMock;
        private IsRegisteredUserQueryHandler _queryhandler;
        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<ProfileController>>();

            _profileController = new ProfileController(_loggerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        RequestServices = new ServiceCollection()
                        .AddScoped(_ => _mediatorMock.Object)
                        .AddScoped(_ => _loggerMock.Object)
                        .BuildServiceProvider()
                    }
                }
            };



            //var users = new List<AppUser>
            //{
            //    new AppUser { UserName = "user1", Email = "test@gmail.com", PhoneNumber = "1234567890" },
            //    new AppUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "9876543210" }
            //};

            //var mockUserStore = Mock.Of<IUserStore<AppUser>>();
            //var mockUserQueryable = users.BuildMock();

            //_userManagerMock = new Mock<UserManager<AppUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            //_userManagerMock.Setup(u => u.Users).Returns(mockUserQueryable);
            //_userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            //    .Returns(Task.FromResult(IdentityResult.Success));
            //_userManagerMock.Setup(userManager => userManager
            //.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            ////_queryhandler = new IsRegisteredUserQueryHandler(_userManagerMock.Object);
        }

        [Test]
        public async Task Get_ProfileLanders_Returns_OkObjectResult_With_Items()
        {
            // Arrange
            string email = "test@gmail.com";
            CancellationToken cancellationToken = default;
            //var query = new GetProfileLandersByEmailQuery(email,cancellationToken);
            var expectedResponse = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = new
                {
                    IsSeller = false,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = "0817123456",
                    PhoneNumberConfirmed = true,
                    Active = true,
                    NewsLetter = true,
                    FirstName = "test full name",
                    LastName = "jika ada last name",
                    ImageUrl = "123456.jpg"
                }
            };

            //_mediatorMock.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(expectedResponse);
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetProfileLandersByEmailQuery>(), CancellationToken.None)).ReturnsAsync(expectedResponse);



            // Act
            var result = await _profileController.Landers(email, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            //Assert.That(okResult.Value, Is.EqualTo(expectedBrands));

            var actualResponse = (BaseWithDataResponse)okResult.Value!; // Memastikan nilai yang dikembalikan adalah BaseWithDataCountResponse
            Assert.Multiple(() =>
            {
                Assert.That(actualResponse.Success, Is.EqualTo(expectedResponse.Success)); // Memastikan nilai Success sesuai
                Assert.That(actualResponse.Message, Is.EqualTo(expectedResponse.Message)); // Memastikan nilai Message sesuai
                Assert.That(actualResponse.Data, Is.EqualTo(expectedResponse.Data)); // Memastikan nilai Data sesuai
            });
        }

        [Test]
        public async Task Get_ProfileMerchant_Returns_OkObjectResult_With_Items()
        {
            // Arrange
            string email = "test@gmail.com";
            CancellationToken cancellationToken = default;
            //var query = new GetProfileLandersByEmailQuery(email,cancellationToken);
            var expectedResponse = new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Get Data",
                Data = new
                {
                    IsSeller = true,
                    IsCompany = true,
                    CompanyName = "Pt.Cobajah",
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = "0817123456",
                    PhoneNumberConfirmed = true,
                    Active = true,
                    SellerCategoryId = "abc1234",
                    BrandName = "Brand akuh",
                    NewsLetter = true,
                    FirstName = "test full name",
                    LastName = "jika ada last name",
                    ImageUrl = "123456.jpg",
                    Address = "",
                    Province = "",
                    City = "",
                    District = "",
                    SubDistrict = "",
                    PostCode = "",
                    WillingContacted = true,
                }
            };

            //_mediatorMock.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(expectedResponse);
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetProfileMerchantByEmailQuery>(), CancellationToken.None))
                .ReturnsAsync(expectedResponse);



            // Act
            var result = await _profileController.Merchant(email, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            //Assert.That(okResult.Value, Is.EqualTo(expectedBrands));

            var actualResponse = (BaseWithDataResponse)okResult.Value!; // Memastikan nilai yang dikembalikan adalah BaseWithDataCountResponse
            Assert.Multiple(() =>
            {
                Assert.That(actualResponse.Success, Is.EqualTo(expectedResponse.Success)); // Memastikan nilai Success sesuai
                Assert.That(actualResponse.Message, Is.EqualTo(expectedResponse.Message)); // Memastikan nilai Message sesuai
                Assert.That(actualResponse.Data, Is.EqualTo(expectedResponse.Data)); // Memastikan nilai Data sesuai
            });
        }

        [Test]
        public async Task Put_ProfileLander_Should_Return_Ok_With_Items()
        {
            // Arrange
            string email = "test@gmail.com";
            var command = new EditProfileLandersCommand()
            {
                Email = email,
                PhoneNumber = "0817123456",
                PhoneNumberConfirmed = true,
                NewsLetter = true,
                FirstName = "test full name",
                LastName = "jika ada last name",
                ImageUrl = "123456.jpg"
            };
            var expectedresponse = new BaseWithDataResponse
            {
                Success = true,
                Message = "",
                Data = new
                {
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = "0817123456",
                    PhoneNumberConfirmed = true,
                }
            };

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(expectedresponse);

            // Act
            var result = await _profileController.Landers(email, command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedresponse));
        }
        [Test]
        public async Task Put_ProfileMerchant_Should_Return_Ok_With_Items()
        {
            // Arrange
            string email = "test@gmail.com";
            string phoneNumber = "0817123456";
            var command = new EditProfileMerchantCommand()
            {
                Email = email,
                SellerCategoryId = "123456",
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true,
                BrandName = "Brand akuh",
                FirstName = "test full name",
                LastName = "jika ada last name",
                ImageUrl = "123456.jpg",
                CompanyName = "Pt.Cobajah",
                Address = "",
                Province = "",
                City = "",
                District = "",
                SubDistrict = "",
                PostCode = "",
                WillingContacted = true,

            };
            var expectedresponse = new BaseWithDataResponse
            {
                Success = true,
                Message = "",
                Data = new
                {
                    Email = email,
                    EmailConfirmed = true,
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = true,
                }
            };

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(expectedresponse);

            // Act
            var result = await _profileController.Merchant(email, command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedresponse));
        }



    }
}
