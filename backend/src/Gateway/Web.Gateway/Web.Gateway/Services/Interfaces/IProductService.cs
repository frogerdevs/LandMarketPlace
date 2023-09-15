namespace Web.Gateway.Services.Interfaces
{
    public interface IProductService
    {
        ValueTask<IEnumerable<ProductsItem>?> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<BasePagingResponse<ProductsByCategoryItemResponse>> GetPagingAsync(PagingProductRequest request, CancellationToken cancellationToken = default);
        ValueTask<ProductItemResponse?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<ProductItemResponse?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default);
        ValueTask<BasePagingResponse<ProductsByCategoryItemResponse>?> GetByCategorySlugAsync(PagingBySlugRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(ProductRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, ProductPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}