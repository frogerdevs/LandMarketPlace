using System.Net;

namespace Catalog.Api.Test
{
    public class CategoryControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<CategoryController>> _loggerMock;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CategoryController>>();

            _categoryController = new CategoryController(_loggerMock.Object)
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
        }

        [Test]
        public async Task Get_Returns_OkObjectResult_With_Items()
        {
            // Arrange
            var expectedResponse = new List<CategoryItemResponse>()
                {
                    new CategoryItemResponse{Id = Guid.NewGuid().ToString(), Name = "Architect", Slug="architect",Active = true, Description = "", ImageUrl="123.png" },
                    new CategoryItemResponse {Id = Guid.NewGuid().ToString(),Name = "Agent Property", Slug="agent-property",Active = true, Description = "", ImageUrl="123.png"}
                };

            //_mediatorMock.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(expectedResponse);
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetCategoriesQuery>(), CancellationToken.None)).ReturnsAsync(expectedResponse);


            // Act
            var result = await _categoryController.Get(CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            //Assert.That(okResult.Value, Is.EqualTo(expectedBrands));

            var actualResponse = (BaseWithDataResponse)okResult.Value!; // Memastikan nilai yang dikembalikan
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(actualResponse.Data, Is.EqualTo(expectedResponse));
            });
        }

        [Test]
        public async Task Post_Should_Return_Ok_With_Items()
        {
            // Arrange
            var command = new AddCategoryCommand() { Name = "Architect", Active = true, Description = "", ImageUrl = "123.png" };
            var expectedresponse = new AddCategoryResponse { Success = true, Message = "", Data = new { Name = "Architect", Active = true, Description = "", ImageUrl = "123.png" } };

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(expectedresponse);

            // Act
            var result = await _categoryController.Post(command, CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedresponse));
        }
    }
}
