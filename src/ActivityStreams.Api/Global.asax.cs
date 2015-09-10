using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;

namespace ActivityStreams.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IActivityRepository ActivityRepository;
        public static IActivityFeedRepository FeedRepository;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ActivityRepository = new InMemoryActivityFeedRepository();
            FeedRepository = new InMemorySubscriptionRepository();
        }
    }
}
