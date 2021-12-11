
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Web.Http.Routing;

namespace PRASYDE.ControlEscolar.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            config.Routes.MapHttpRoute(
              "PostBlobUpload",
              "blobs/upload",
              new { controller = "Imagenes", action = "PostBlobUpload" },
              new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );
        }
    }
}
