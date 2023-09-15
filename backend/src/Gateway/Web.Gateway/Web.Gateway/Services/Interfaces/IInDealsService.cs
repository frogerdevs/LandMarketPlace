namespace Web.Gateway.Services.Interfaces
{
    public interface IInDealsService
    {
        ValueTask<HomeDealsResponse> GetForHomePageAsync(CancellationToken cancellationToken = default);
        ValueTask<HomeDealsResponse> GetDealsItemAsync(string id);
        ValueTask<BasePagingResponse<InDealsItem>> GetInDealsAsync(InDealsRequest request, CancellationToken cancellationToken = default);
    }
}
