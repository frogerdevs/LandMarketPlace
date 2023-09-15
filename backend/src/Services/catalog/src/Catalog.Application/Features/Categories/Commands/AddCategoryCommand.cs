namespace Catalog.Application.Features.Categories.Commands
{
    public partial class AddCategoryCommand : ICommand<AddCategoryResponse>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, AddCategoryResponse>
    {
        private readonly IBaseRepositoryAsync<Category, string> _categoryRepo;
        readonly SlugHelper slugHelper;

        public AddCategoryCommandHandler(IBaseRepositoryAsync<Category, string> categoryRepo)
        {
            slugHelper = new();
            _categoryRepo = categoryRepo;
        }

        public async ValueTask<AddCategoryResponse> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug(command.Name);
            var category = MapToCategory(command, slug);
            var res = await _categoryRepo.AddAsync(category, cancellationToken);
            var dataResult = MapToItemResponse(res);
            return new AddCategoryResponse
            {
                Success = true,
                Message = "Success Create Category",
                Data = dataResult
            };
        }

        private static CategoryItemResponse MapToItemResponse(Category res)
        {
            return new CategoryItemResponse
            {
                Id = res.Id,
                Name = res.Name,
                Slug = res.Slug,
                Description = res.Description,
                ImageUrl = res.ImageUrl,
                Active = res.Active
            };
        }

        private static Category MapToCategory(AddCategoryCommand command, string slug)
        {
            return new Category()
            {
                Name = command.Name,
                Slug = slug,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Active = command.Active
            };
        }
    }
}
