namespace Subscription.Application.Features.UnitTypes.Commands
{
    public partial class AddUnitTypeCommand : ICommand<UnitTypeItem?>
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public bool Active { get; set; }
        public string? CreatedBy { get; set; }
    }
    public class AddUnitTypeCommandHandler : ICommandHandler<AddUnitTypeCommand, UnitTypeItem?>
    {
        private readonly IBaseRepositoryAsync<BenefitType, string> _repo;

        public AddUnitTypeCommandHandler(IBaseRepositoryAsync<BenefitType, string> categoryRepo)
        {
            _repo = categoryRepo;
        }

        public async ValueTask<UnitTypeItem?> Handle(AddUnitTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = MapToEntity(command);
            var res = await _repo.AddAsync(entity, cancellationToken);
            return MapToItemResponse(res);
        }

        private static BenefitType MapToEntity(AddUnitTypeCommand command)
        {
            return new BenefitType
            {
                Name = command.Name,
                Description = command.Description,
                Size = command.Size,
                Active = command.Active,
                CreatedBy = command.CreatedBy,
            };
        }

        private static UnitTypeItem MapToItemResponse(BenefitType res)
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
