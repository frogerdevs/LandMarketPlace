namespace Subscription.Application.Features.UnitTypes.Commands
{
    public partial class EditUnitTypeCommand : ICommand<UnitTypeItem?>
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public bool Active { get; set; }
        public string? UpdateBy { get; set; }
    }
    public class EditUnitTypeCommandHandler : ICommandHandler<EditUnitTypeCommand, UnitTypeItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitType, string> _repo;
        public EditUnitTypeCommandHandler(IBaseRepositoryAsync<BenefitType, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<UnitTypeItem?> Handle(EditUnitTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (entity == null)
            {
                return null!;
            }
            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.Size = command.Size;
            entity.Active = command.Active;
            entity.UpdatedBy = command.UpdateBy;

            var res = await _repo.UpdateAsync(entity, entity.Id, cancellationToken);
            return MaptoResponse(res);
        }

        private static UnitTypeItem MaptoResponse(BenefitType res)
        {
            return new UnitTypeItem
            {
                Id = res.Id,
                Name = res.Name,
                Description = res.Description,
                Size = res.Size,
                Active = res.Active
            };
        }
    }

}
