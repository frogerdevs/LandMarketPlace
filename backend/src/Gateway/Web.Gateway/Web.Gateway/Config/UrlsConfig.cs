namespace Web.Gateway.Config
{
    public class UrlsConfig
    {
        public class CatalogService
        {
            public static string DealsForHome() => "/api/v1/home/deals";
            public static string Deals() => "/api/v1/deals";
            public static string DealById(string id) => $"/api/v1/deal/{id}";
            public static string CategoryForHome() => "/api/v1/home/category";
            public static string Category() => "/api/v1/category";
            public static string Category(string id) => $"/api/v1/category/{id}";
            public static string CategoryBySlug(string slug) => $"/api/v1/category/byslug/{slug}";
            public static string SubCategory() => "/api/v1/subcategory";
            public static string SubCategory(string id) => $"/api/v1/subcategory/{id}";
            public static string SubCategoryBySlug(string slug) => $"/api/v1/subcategory/byslug/{slug}";
            public static string Certificate() => "/api/v1/certificate";
            public static string Certificate(string id) => $"/api/v1/certificate/{id}";
            public static string Facility() => "/api/v1/facility";
            public static string Facility(string id) => $"/api/v1/facility/{id}";
            public static string Product() => "/api/v1/product";
            //public static string ProductPaging(int pagenumber, int pagesize) => $"/api/v1/product/paging?pagenumber={pagenumber}&pagesize={pagesize}";
            public static string ProductPaging(string query) => $"/api/v1/product/paging?{query}";
            public static string Product(string id) => $"/api/v1/product/{id}";
            public static string ProductByUser(string userid) => $"/api/v1/product/ByUser/{userid}";
            public static string ProductBySlug(string slug) => $"/api/v1/product/BySlug/{slug}";
            public static string ProductByCategorySlug(string slug) => $"/api/v1/product/ByCategorySlug/{slug}";
            public static string ProductDiscount() => "/api/v1/ProductDiscount";
            public static string ProductDiscount(string id) => $"/api/v1/ProductDiscount/{id}";
            public static string ProductDiscountActivate(string id) => $"/api/v1/ProductDiscount/Activate/{id}";
            public static string ProductDiscountBySlug(string slug) => $"/api/v1/ProductDiscount/BySlug/{slug}";
            public static string ProductDiscountByUser(string userid) => $"/api/v1/ProductDiscount/ByUser/{userid}";
            public static string Adsense() => "/api/v1/Adsense";
            public static string Adsense(string id) => $"/api/v1/Adsense/{id}";
            public static string AdsenseBySlug(string slug) => $"/api/v1/Adsense/BySlug/{slug}";
            public static string AdsenseByUser(string userid) => $"/api/v1/Adsense/ByUser/{userid}";
            public static string HomeDeal() => "/api/v1/HomeDeal";
            public static string HomeDealQuery(string query) => $"/api/v1/HomeDeal?{query}";
            public static string HomeDeal(string id) => $"/api/v1/HomeDeal/{id}";
            public static string HomeDealActivate(string id) => $"/api/v1/HomeDeal/activate/{id}";
            public static string HomeDealBySlug(string slug) => $"/api/v1/HomeDeal/BySlug/{slug}";
            public static string HomeDealByUser(string userid) => $"/api/v1/HomeDeal/ByUser/{userid}";
        }
        public class IdentityService
        {
            public static string Token() => "/connect/token";
            public static string Signout() => "/connect/logout";
            public static string RegisterLanders() => "/api/v1/Auth/RegisterLanders";
            public static string RegisterMerchant() => "/api/v1/Auth/RegisterMerchant";
            public static string ForgotPassword() => "/api/v1/Auth/ForgotPassword";
            public static string ResetPassword() => "/api/v1/Auth/ResetPassword";
            //public static string Signout() => "api/v1/auth/SignOut";
            //public static string GetCsrf() => "connect/CsrfToken";
            public static string GetCsrf() => "api/v1/auth/CsrfToken";
            public static string GetUserInfo() => "connect/userinfo";
            public static string User() => "/api/v1/user";
            public static string UserById(string id) => $"/api/v1/user/{id}";
            public static string UserByEmail(string email) => $"/api/v1/user/ByEmail/{email}";
            public static string UserIsRegistered(string emailorphone) => $"/api/v1/user/IsRegistered/{emailorphone}";
            public static string Province() => "/api/v1/province";
            public static string ProvinceById(string id) => $"/api/v1/province/{id}";
            public static string City() => "/api/v1/city";
            public static string CityById(string id) => $"/api/v1/city/{id}";
            public static string CityByProvinceId(string provinceid) => $"/api/v1/city/byprovince/{provinceid}";
            public static string District() => "/api/v1/district";
            public static string DistrictById(string id) => $"/api/v1/district/{id}";
            public static string DistrictByCityId(string cityid) => $"/api/v1/district/bycity/{cityid}";
            public static string SubDistrict() => "/api/v1/subdistrict";
            public static string SubDistrictById(string id) => $"/api/v1/subdistrict/{id}";
            public static string SubDistrictByDistrictId(string districtid) => $"/api/v1/subdistrict/bydistrict/{districtid}";
            public static string ProfileLanders() => $"/api/v1/profile/Landers";
            public static string ProfileMerchant() => $"/api/v1/profile/Merchant";
            public static string ProfileMerchantVerification() => $"/api/v1/profile/MerchantVerification";
            public static string ProfileChangePassword() => $"/api/v1/profile/ChangePassword";
            public static string ProfileLanders(string email) => $"/api/v1/profile/Landers/{email}";
            public static string ProfileMerchant(string email) => $"/api/v1/profile/Merchant/{email}";
            public static string SendOtp() => $"/api/v1/otp/SendOtp";
            public static string VerifyOtp() => $"/api/v1/otp/VerifyOtp";
            public static string About(string email) => $"/api/v1/about/{email}";
        }
    }
}
