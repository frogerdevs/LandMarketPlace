using Catalog.Application.Dtos.Response.Category;
using Catalog.Application.Features.Categories.Commands;
using Catalog.Application.Interfaces.Repositories.Base;
using Catalog.Domain.Entities.Categories;
using Moq;

namespace Catalog.Application.Test
{
    [TestFixture]
    public class AddCategoryCommandHandlerTests
    {
        private Mock<IBaseRepositoryAsync<Category, string>> _mockCategoryRepository;
        private AddCategoryCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockCategoryRepository = new Mock<IBaseRepositoryAsync<Category, string>>();
            _handler = new AddCategoryCommandHandler(_mockCategoryRepository.Object);
        }

        [Test]
        public async Task Handle_WithValidCommand_ReturnsBrandResponse()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "TestCategory",
                Slug = "testcategory",
                Active = true,
                Description = "Description",
                ImageUrl = "1234.png",

            };
            //var brand = new Brand
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    BrandName = "TestBrand"
            //};
            _mockCategoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>(), default))
                .ReturnsAsync(category);

            var command = new AddCategoryCommand
            {
                Name = "Name",
                Active = true,
                Description = "Description",
                ImageUrl = "1234.png"
            };

            // Act
            var result = await _handler.Handle(command, default);
            Assert.Multiple(() =>
            {

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(((CategoryItemResponse)result.Data!).Name, Is.EqualTo(category.Name));
            });
        }
    }
}
