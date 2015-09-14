using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using System.Web.Http;

namespace ActivityStreams.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IActivityRepository ActivityRepository = new InMemoryActivityRepository();
        public static IActivityFeedRepository FeedRepository = new InMemoryFeedRepository();

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

    }
}
