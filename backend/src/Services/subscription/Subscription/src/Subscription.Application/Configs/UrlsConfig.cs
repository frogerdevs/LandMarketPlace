namespace Subscription.Application.Configs
{
    public static class UrlsConfig
    {
        public static class NotificationApi
        {
            public static string GetAllTemplate() => $"api/v1/TemplateNotification";
            public static string GetTemplateByRequestType(string requesttype) => $"api/v1/TemplateNotification/{requesttype}";

        }

        public static class CoreApi
        {
            public static string CodeCounter() => $"api/v1/CodeCounter";
        }
    }
}
