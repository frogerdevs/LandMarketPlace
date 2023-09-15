namespace Catalog.Application.Features.SubCategories.Command
{
    public partial class EditSubCategoryCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }
    public class EditSubCategoryCommandHandler : ICommandHandler<EditSubCategoryCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _subcategoryRepo;
        readonly SlugHelper slugHelper;
        public EditSubCategoryCommandHandler(IBaseRepositoryAsync<SubCategory, string> subcategoryRepo)
        {
            slugHelper = new();
            _subcategoryRepo = subcategoryRepo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(EditSubCategoryCommand command, CancellationToken cancellationToken)
        {
            var slug = slugHelper.GenerateSlug(command.Name);
            var subcategory = await _subcategoryRepo.GetByIdAsync(command.Id, cancellationToken);
            if (subcategory == null)
            {
                return null!;
            }
            subcategory.CategoryId = command.Id;
            subcategory.Name = command.Name;
            subcategory.Description = command.Description;
            subcategory.Active = command.Active;
            subcategory.ImageUrl = command.ImageUrl;
            subcategory.Slug = slug;

            var res = await _subcategoryRepo.UpdateAsync(subcategory, subcategory.Id, cancellationToken);

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Edit SubCategory",
                Data = res
            };
        }
    }

}
