using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Api.Controllers.v1.Brands;
using Ordering.Application.Dtos.Respons.Base;
using Ordering.Application.Dtos.Respons.Brands;
using Ordering.Application.Features.Brands.Commands.AddEdit;
using Ordering.Application.Features.Brands.Queries;
using Ordering.Domain.Entities.Brands;

namespace Ordering.Api.Test
{
    public class BrandControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<BrandController>> _loggerMock;
        private BrandController _brandController;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<BrandController>>();

            _brandController = new BrandController(_loggerMock.Object)
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
            var query = new GetAllBrandQuery();
            var expectedBrands = new BaseWithDataResponse
            {
                IsSuccess = true,
                Message = "Success Get Data",
                Data = new List<Brand>()
                {
                    new Brand { BrandName = "Brand 1" },
                    new Brand {BrandName = "Brand 2"}
                }
            };

            _mediatorMock.Setup(x => x.Send(query, CancellationToken.None)).ReturnsAsync(expectedBrands);

            // Act
            var result = await _brandController.Get(CancellationToken.None);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedBrands));
        }

        [Test]
        public async Task Post_Should_Return_Ok_With_Items()
        {
            // Arrange
            var command = new AddBrandCommand() { BrandName = "Brand 1" };
            var expectedresponse = new BrandResponse { IsSuccess = true, BrandName = "Brand 1" };

            _mediatorMock.Setup(x => x.Send(command, CancellationToken.None)).ReturnsAsync(expectedresponse);

            // Act
            var result = await _brandController.Post(command);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(expectedresponse));
        }
    }
}
