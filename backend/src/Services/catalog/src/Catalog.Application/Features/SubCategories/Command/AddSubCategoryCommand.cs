namespace Catalog.Application.Features.SubCategories.Command
{
    public partial class AddSubCategoryCommand : ICommand<BaseWithDataResponse>
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class AddSubCategoryCommandHandler : ICommandHandler<AddSubCategoryCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _subcategoryRepo;
        readonly SlugHelper slugHelper;

        public AddSubCategoryCommandHandler(IBaseRepositoryAsync<SubCategory, string> categoryRepo)
        {
            slugHelper = new();
            _subcategoryRepo = categoryRepo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(AddSubCategoryCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug(command.Name);
            var subcategory = MapToSubCategory(command, slug);
            var res = await _subcategoryRepo.AddAsync(subcategory, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Create SubCategory",
                Data = res
            };
        }

        private SubCategory MapToSubCategory(AddSubCategoryCommand command, string slug)
        {
            return new SubCategory
            {
                CategoryId = command.CategoryId,
                Name = command.Name,
                Slug = slug,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Active = command.Active
            };
        }
    }

}
