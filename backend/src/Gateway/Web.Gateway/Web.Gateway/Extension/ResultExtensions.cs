using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Web.Gateway.Extension
{
    internal static class ResultExtensions
    {
        internal static async Task<T?> ToResult<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            T? result = default;
            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(responseAsString))
                {
                    return result;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                //options.Converters.Add(new DateOnlyConverter());
                //options.Converters.Add(new TimeOnlyConverter());
                //options.Converters.Add(new DateTimeConverter());

                result = JsonSerializer.Deserialize<T>(responseAsString, options);
            }
            return result;

        }

        internal static async ValueTask<ContentResult> ToActionResultAsync(this HttpResponseMessage httpResponse, CancellationToken cancellationToken = default)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            //return new ObjectResult(responseContent)
            //{
            //    StatusCode = (int)httpResponse.StatusCode,

            //};
            return new ContentResult
            {
                Content = responseContent,
                StatusCode = (int)httpResponse.StatusCode,
                ContentType = "application/json" // Sesuaikan dengan jenis konten yang benar jika perlu
            };
        }
    }
}
