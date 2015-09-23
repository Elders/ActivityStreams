using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;
using System.Web.Http;

namespace ActivityStreams.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IActivityRepository ActivityRepository = new InMemoryActivityRepository();
        public static FeedFactory FeedFactory = new FeedFactory(new FeedStreamRepository(new InMemoryFeedStreamStore()));

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

    }
}
