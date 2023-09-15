using IdentityServer.Data.Entites;
using IdentityServer.Features.Users.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;


namespace Identity.Api.Test.Features.Users.Queries
{
    public class IsRegisteredUserQueryTests
    {
        private Mock<UserManager<AppUser>> _mockRepository;
        private IsRegisteredUserQueryHandler _handler;

        private Mock<UserManager<AppUser>> _userManagerMock;
        private IsRegisteredUserQueryHandler _queryhandler;
        [SetUp]
        public void SetUp()
        {

            _mockRepository = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<AppUser>>().Object,
                new IUserValidator<AppUser>[0],
                new IPasswordValidator<AppUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<AppUser>>>().Object);
            _handler = new IsRegisteredUserQueryHandler(_mockRepository.Object);


            var users = new List<AppUser>
            {
                new AppUser { UserName = "user1", Email = "user1@example.com", PhoneNumber = "1234567890" },
                new AppUser { UserName = "user2", Email = "user2@example.com", PhoneNumber = "9876543210" }
            };

            var mockUserStore = Mock.Of<IUserStore<AppUser>>();
            var mockUserQueryable = users.BuildMock();

            _userManagerMock = new Mock<UserManager<AppUser>>(mockUserStore, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(u => u.Users).Returns(mockUserQueryable);
            _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            _userManagerMock.Setup(userManager => userManager
            .AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
            _queryhandler = new IsRegisteredUserQueryHandler(_userManagerMock.Object);

        }

        [Test]
        public async Task TestMethod1()
        {
            // Arrange
            string testEmail = "user1@example.com";
            var query = new IsRegisteredUserQuery { EmailOrPhone = testEmail };

            // Act
            bool result = await _queryhandler.Handle(query, CancellationToken.None);


            // Assert
            Assert.IsTrue(result);


        }
    }
}
