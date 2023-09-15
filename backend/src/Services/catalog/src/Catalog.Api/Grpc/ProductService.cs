using Google.Protobuf.WellKnownTypes;

namespace Catalog.Api.Grpc
{
    public class ProductService : ProductGrpc.ProductGrpcBase
    {
        readonly IMediator _mediator;
        public ProductService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<GrpcPagingProductResponse> GetIndeals(GrpcPagingIndealsRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetPagingProductsQuery()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Active = true,
                Search = request.Search,
            });

            GrpcPagingProductResponse res = new()
            {
                TotalData = items.TotalData,
                CurrentPage = items.CurrentPage,
                Limit = items.Limit,
                Count = items.Count,
            };
            res.Data.AddRange(items.Data?.Select(item => MaptoProductItem(item)));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcProductResponse> GetItems(GrpcEmptyRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProductsQuery() { });
            GrpcProductResponse res = new();
            res.Data.AddRange(item?.Select(item => MaptoProductItem(item)));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcPagingProductResponse> GetPagingItems(GrpcPagingProductRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetPagingProductsQuery()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Active = true,
                UserId = request.UserId,
                CategoryName = request.CategoryName,
                Price = Convert.ToDecimal(request.Price),
                ProductName = request.ProductName,
            });

            GrpcPagingProductResponse res = new()
            {
                TotalData = items.TotalData,
                CurrentPage = items.CurrentPage,
                Limit = items.Limit,
                Count = items.Count,
            };
            res.Data.AddRange(items.Data?.Select(item => MaptoProductItem(item)));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        public override async Task<GrpcProductsByCategoryResponse> GetItemsByCategorySlug(GrpcPagingBySlugRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProductsByCategorySlugQuery()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Slug = request.Slug
            });

            GrpcProductsByCategoryResponse res = new()
            {
                Success = item.Success,
                Message = item.Message,
                TotalData = item.TotalData,
                CurrentPage = item.CurrentPage,
                Limit = item.Limit,
                Count = item.Count
            };
            if (item.Data != null)
                res.Data.AddRange(item?.Data?.Select(item => MaptoProductItemByCategory(item)));
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }


        public override async Task<GrpcProductItemResponse> GetItemById(GrpcByIdRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProductByIdQuery() { Id = request.Id });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcProductItemResponse();
            }
            var res = MapToProductItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }
        public override async Task<GrpcProductItemResponse> GetItemBySlug(GrpcBySlugRequest request, ServerCallContext context)
        {
            var item = await _mediator.Send(new GetProductBySlugQuery() { Slug = request.Slug });
            if (item == null)
            {
                context.Status = new Status(StatusCode.NotFound, $"Data tidak ditemukan");
                return new GrpcProductItemResponse();
            }
            var res = MapToProductItemResponse(item);
            context.Status = new Status(StatusCode.OK, $"Success");

            return res;
        }

        #region Method
        private static GrpcProductsByCategoryItemResponse? MaptoProductItemByCategory(ProductsByCategoryItemResponse? item)
        {
            return item != null ? new GrpcProductsByCategoryItemResponse
            {
                Id = item.Id ?? "",
                UserId = item.UserId ?? "",
                CategoryId = item.CategoryId ?? "",
                CategoryName = item.CategoryName ?? "",
                CategorySlug = item.CategorySlug ?? "",
                Title = item.Title,
                Slug = item.Slug,
                Province = item.Province ?? "",
                City = item.City ?? "",
                District = item.District ?? "",
                PriceFrom = (double)item.PriceFrom,
                PriceTo = (double)item.PriceTo,
                Active = item.Active,
                ImageUrl = item.ImageUrl ?? "",
            } : null;
        }
        private static GrpcProductItemResponse MapToProductItemResponse(ProductItemResponse item)
        {
            var res = new GrpcProductItemResponse
            {
                Id = item.Id,
                UserId = item.UserId,
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName ?? "",
                SubCategoryId = item.SubCategoryId ?? "",
                SubCategoryName = item.SubCategoryName ?? "",
                Title = item.Title,
                Slug = item.Slug,
                Province = item.Province ?? "",
                City = item.City ?? "",
                District = item.District ?? "",
                SubDistrict = item.SubDistrict ?? "",
                PostCode = item.PostCode ?? "",
                Address = item.Address ?? "",
                CertificateId = item.CertificateId ?? "",
                RegisteredSince = Timestamp.FromDateTimeOffset((item.RegisteredSince ?? DateTime.MinValue)),
                PriceFrom = (double)item.PriceFrom,
                PriceTo = (double)item.PriceTo,
                Description = item.Description ?? "",
                Details = item.Details ?? "",
                LocationLongitude = item.LocationLongitude ?? "",
                LocationLatitude = item.LocationLatitude ?? "",
                Active = item.Active,
            };
            res.ProductImages.AddRange(item.ProductImages?.Select(item => MaptoProductImage(item)));
            res.ProductFacilities.AddRange(item.ProductFacilities?.Select(item => MaptoProductFacilities(item)));
            res.ProductNears.AddRange(item.ProductNears?.Select(item => MaptoProductNears(item)));
            res.ProductSpecifications.AddRange(item.ProductSpecifications?.Select(item => MaptoProductSpecifications(item)));
            return res;
        }

        private static GrpcProductSpecificationResponse MaptoProductSpecifications(ProductSpecificationResponse item)
        {
            return new GrpcProductSpecificationResponse
            {
                ProductId = item.ProductId,
                Title = item.Title ?? "",
                Description = item.Description ?? ""
            };
        }

        private static GrpcProductNearResponse MaptoProductNears(ProductNearResponse item)
        {
            var res = new GrpcProductNearResponse
            {
                ProductId = item.ProductId,
                Title = item.Title ?? ""
            };
            res.ProductNearItems.AddRange(item.ProductNearItems?.Select(item => MaptoProductNearItems(item)));
            return res;
        }

        private static GrpcProductNearItemResponse MaptoProductNearItems(ProductNearItemResponse item)
        {
            return new GrpcProductNearItemResponse
            {
                ProductId = item.ProductId,
                ProductNearId = item.ProductNearId,
                Title = item.Title ?? ""
            };
        }

        private static GrpcProductFacilityResponse MaptoProductFacilities(ProductFacilityResponse item)
        {
            return new GrpcProductFacilityResponse
            {
                ProductId = item.ProductId,
                FacilityId = item.FacilityId,
                FacilityName = item.FacilityName ?? ""
            };
        }

        private static GrpcProductImageItemResponse MaptoProductImage(ProductImageItemResponse item)
        {
            return new GrpcProductImageItemResponse
            {
                ProductId = item.ProductId,
                ImageUrl = item.ImageUrl ?? "",
                ImageType = item.ImageType ?? "",
                ImageName = item.ImageName ?? "",
            };
        }

        private static GrpcProductsItem MaptoProductItem(ProductsItem item)
        {
            return new GrpcProductsItem
            {
                Id = item.Id,
                UserId = item.UserId,
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName,
                CategorySlug = item.CategorySlug,
                Title = item.Title,
                Slug = item.Slug,
                Address = item.Address,
                City = item.City,
                District = item.District,
                PriceFrom = (double)item.PriceFrom,
                PriceTo = (double)item.PriceTo,
                ImageUrl = item.ImageUrl,
                Active = item.Active
            };
        }
        #endregion
    }
}
