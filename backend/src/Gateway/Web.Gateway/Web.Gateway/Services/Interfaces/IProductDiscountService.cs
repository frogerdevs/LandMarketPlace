namespace Web.Gateway.Services.Interfaces
{
    public interface IProductDiscountService
    {
        ValueTask<IEnumerable<ProductDiscountItem>> GetAsync(CancellationToken cancellationToken = default);
        ValueTask<ProductDiscountItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        ValueTask<IEnumerable<ProductDiscountByCategorySlugItem>?> GetByCategorySlugAsync(string categoryslug, CancellationToken cancellationToken = default);
        ValueTask<IEnumerable<ProductDiscountsOfTheWeekItem>?> GetOfTheweekAsync(CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> GetByUserAsync(string userid, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PostAsync(ProductDiscountRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> PutAsync(string id, ProductDiscountPutRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> ActivateAsync(string id, ActivateProductDiscountRequest request, CancellationToken cancellationToken = default);
        ValueTask<HttpResponseMessage> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
