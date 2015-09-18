using System.Web.Http;
using System.Web.Http.Description;

namespace ActivityStreams.Api.Help
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var theApiConfig = new HttpConfiguration(); // New config object to represent your API's config
            Api.WebApiConfig.Register(theApiConfig); // Configure via WebApiConfig in your API DLL
            theApiConfig.EnsureInitialized();

            config.Services.Replace(typeof(IApiExplorer), new ApiExplorer(theApiConfig));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
