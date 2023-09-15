using System.Reflection;
using System.Web;

namespace Web.Gateway.Extension
{
    internal static class QueryStringBuilderExtension<T>
    {
        internal static string BuildQueryString(T parameters)
        {
            var query = new List<string>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(parameters);

                if (value != null)
                {
                    string propertyName = prop.Name;

                    // Check if the property has a JsonPropertyName attribute
                    var jsonPropertyNameAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (jsonPropertyNameAttribute != null)
                    {
                        propertyName = jsonPropertyNameAttribute.Name;
                    }

                    var encodedValue = HttpUtility.UrlEncode(value.ToString());
                    query.Add($"{propertyName}={encodedValue}");
                }
            }

            return string.Join("&", query);
        }
    }
}
