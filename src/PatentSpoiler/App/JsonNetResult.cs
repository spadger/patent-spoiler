using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PatentSpoiler.App
{
    public class JsonNetResult : JsonResult
    {
        private static JsonSerializerSettings defaultSettings;
        
        static JsonNetResult()
        {
            defaultSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public JsonSerializerSettings Settings { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON GET is not allowed");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data == null)
                return;

            var scriptSerializer = JsonSerializer.Create(Settings ?? defaultSettings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }

    public static class JsonNetResultExtensions
    {
        public static JsonNetResult JsonNetResult(this Controller @this, object data)
        {
            return JsonNetResult(@this, data, JsonRequestBehavior.DenyGet, null);
        }

        public static JsonNetResult JsonNetResult(this Controller @this, object data, JsonRequestBehavior requestBehavior)
        {
            return JsonNetResult(@this, data, requestBehavior, null);
        }

        public static JsonNetResult JsonNetResult(this Controller @this, object data, JsonRequestBehavior requestBehavior, JsonSerializerSettings settings)
        {
            return new JsonNetResult { Data = data, JsonRequestBehavior = requestBehavior, Settings = settings };
        }
    }
}