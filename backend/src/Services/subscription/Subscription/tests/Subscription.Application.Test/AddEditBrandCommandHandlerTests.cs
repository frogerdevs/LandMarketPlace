namespace Subscription.Application.Test
{
    [TestFixture]
    public class AddEditBrandCommandHandlerTests
    {
        //private Mock<IBaseRepositoryAsync<Brand, string>> _mockBrandRepository;
        //private AddEditBrandCommandHandler _handler;

        //[SetUp]
        //public void SetUp()
        //{
        //    _mockBrandRepository = new Mock<IBaseRepositoryAsync<Brand, string>>();
        //    _handler = new AddEditBrandCommandHandler(_mockBrandRepository.Object);
        //}

        //[Test]
        //public async Task Handle_WithValidCommand_ReturnsBrandResponse()
        //{
        //    // Arrange
        //    var brand = new Brand
        //    {
        //        BrandName = "TestBrand"
        //    };
        //    _mockBrandRepository.Setup(x => x.AddAsync(It.IsAny<Brand>(), default))
        //        .ReturnsAsync(brand);

        //    var command = new AddBrandCommand
        //    {
        //        BrandName = "TestBrand"
        //    };

        //    // Act
        //    var result = await _handler.Handle(command, default);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.BrandName, Is.EqualTo(brand.BrandName));
        //}
    }
}
