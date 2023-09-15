namespace Catalog.Application.Features.SubCategories.Command
{
    public class DeleteSubCategoryCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }
    public sealed class DeleteSubCategoryCommandhandler : IRequestHandler<DeleteSubCategoryCommand, bool>
    {
        private readonly IBaseRepositoryAsync<SubCategory, string> _repo;

        public DeleteSubCategoryCommandhandler(IBaseRepositoryAsync<SubCategory, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<bool> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the entity to be deleted from the database
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            // Remove the entity from the database
            await _repo.DeleteAsync(entity, cancellationToken);

            return true; // Return a completed Task
        }
    }
}
