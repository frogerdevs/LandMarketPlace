namespace Web.Gateway.Services.Interfaces
{
    public interface ISubCategoryService
    {
        ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(SubCategoryRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, SubCategoryPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);

    }
}
