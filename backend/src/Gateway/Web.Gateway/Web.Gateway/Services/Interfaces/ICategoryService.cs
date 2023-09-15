namespace Web.Gateway.Services.Interfaces
{
    public interface ICategoryService
    {
        ValueTask<CategoriesResponse> GetForHomePageAsync();
        ValueTask<HttpResponseMessage> GetAsync(CancellationToken cancellationToken);
        ValueTask<BaseListResponse<CategoryItem>> GetGrpcAsync(CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> GetByIdAsync(string id, CancellationToken cancellationToken);
        ValueTask<CategoryItem?> GetGrpcByIdAsync(string id, CancellationToken cancellationToken);
        ValueTask<CategoryItem?> GetBySlugAsync(string slug, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PostAsync(CategoryRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> PutAsync(string id, CategoryPutRequest request, CancellationToken cancellationToken);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken);

    }
}
