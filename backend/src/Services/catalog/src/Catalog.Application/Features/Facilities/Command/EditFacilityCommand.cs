namespace Catalog.Application.Features.Facilities.Command
{
    public partial class EditFacilityCommand : ICommand<BaseWithDataResponse>
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool Active { get; set; }
    }
    public class EditFacilityCommandHandler : ICommandHandler<EditFacilityCommand, BaseWithDataResponse>
    {
        private readonly IBaseRepositoryAsync<Facility, string> _repo;
        public EditFacilityCommandHandler(IBaseRepositoryAsync<Facility, string> repo)
        {
            _repo = repo;
        }

        public async ValueTask<BaseWithDataResponse> Handle(EditFacilityCommand command, CancellationToken cancellationToken)
        {
            var certificateType = await _repo.GetByIdAsync(command.Id, cancellationToken);
            if (certificateType == null)
            {
                return null!;
            }
            certificateType.Code = command.Code;
            certificateType.Name = command.Name;
            certificateType.Active = command.Active;

            var res = await _repo.UpdateAsync(certificateType, certificateType.Id, cancellationToken);

            return new BaseWithDataResponse
            {
                Success = true,
                Message = "Success Edit Facility",
                Data = res
            };
        }
    }
}
