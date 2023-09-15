namespace Catalog.Application.Features.Categories.Commands
{
    public partial class EditCategoryCommand : ICommand<EditCategoryResponse>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class EditCategoryCommandHandler : ICommandHandler<EditCategoryCommand, EditCategoryResponse>
    {
        private readonly IBaseRepositoryAsync<Category, string> _categoryRepo;
        readonly SlugHelper slugHelper;
        public EditCategoryCommandHandler(IBaseRepositoryAsync<Category, string> categoryRepo)
        {
            slugHelper = new();
            _categoryRepo = categoryRepo;
        }

        public async ValueTask<EditCategoryResponse> Handle(EditCategoryCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug(command.Name);
            var category = await _categoryRepo.GetByIdAsync(command.Id, cancellationToken);
            if (category == null)
            {
                return null!;
            }
            category.Name = command.Name;
            category.Description = command.Description;
            category.Active = command.Active;
            category.ImageUrl = command.ImageUrl;
            category.Slug = slug;

            var res = await _categoryRepo.UpdateAsync(category, category.Id, cancellationToken);
            var categoryDto = MapToCategoryDto(res);

            return new EditCategoryResponse
            {
                Success = true,
                Message = "Success Edit Category",
                Data = categoryDto
            };
        }

        private static CategoryItemResponse MapToCategoryDto(Category res)
        {
            return new CategoryItemResponse
            {
                Id = res.Id,
                Name = res.Name,
                Slug = res.Slug,
                ImageUrl = res.ImageUrl,
                Description = res.Description,
                Active = res.Active
            };
        }
    }
}