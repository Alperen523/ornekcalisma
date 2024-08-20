using System.Web.Http;
using UzmanCrm.CrmService.Infrastructure.Formatter;

namespace UzmanCrm.CrmService.WebAPI
{
    public static class FormatterConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.Converters
                .Add(new TrimmingConverter());

        }
    }
}