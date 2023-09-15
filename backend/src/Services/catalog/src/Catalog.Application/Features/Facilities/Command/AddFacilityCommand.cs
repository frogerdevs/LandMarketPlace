namespace Catalog.Application.Features.Facilities.Command
{
    public partial class AddFacilityCommand : ICommand<BaseWithDataResponse>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
    public class AddFacilityCommandHandler : ICommandHandler<AddFacilityCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Facility, string> _repo;

        public AddFacilityCommandHandler(IBaseRepositoryAsync<Facility, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(AddFacilityCommand command, CancellationToken cancellationToken)
        {
            var facility = MapToEntity(command);
            var res = await _repo.AddAsync(facility, cancellationToken);
            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Create Facility",
                Data = res
            };
        }

        private static Facility MapToEntity(AddFacilityCommand command)
        {
            return new Facility
            {
                Code = command.Code,
                Name = command.Name,
                Active = command.Active,
            };
        }
    }

}
