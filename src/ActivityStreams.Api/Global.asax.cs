using System.Web.Http;
using ActivityStreams.Persistence;
using ActivityStreams.Persistence.InMemory;

namespace ActivityStreams.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        static IActivityStore ActivityStore = new InMemoryActivityStore();
        static IStreamStore StreamStore = new InMemoryStreamStore();
        static IStreamRepository StreamRepository = new DefaultStreamRepository(StreamStore);

        public static IActivityRepository ActivityRepository = new DefaultActivityRepository(ActivityStore, StreamStore);
        public static StreamService StreamService = new StreamService(StreamRepository);

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

    }
}
