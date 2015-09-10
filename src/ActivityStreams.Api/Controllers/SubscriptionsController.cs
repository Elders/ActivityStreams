using System;
using System.Text;
using System.Web.Http;
using ActivityStreams.Persistence;
using Elders.Web.Api;

namespace ActivityStreams.Api.Controllers
{
    public class SubscriptionsController : ApiController
    {
        ISubscriptionRepository SubscriptionsRepository;

        public IHttpActionResult Subscribe(byte[] streamId, byte[] ownerId)
        {
            var currentSubscription = SubscriptionsRepository.LoadOwnerSubscription(ownerId);

            if (currentSubscription == null)
                currentSubscription = new Subscription(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()), ownerId);

            currentSubscription.SubscribeTo(streamId);
            SubscriptionsRepository.Save(currentSubscription);

            return this.Accepted(currentSubscription.Id);
        }

        public IHttpActionResult UnSubscribe(byte[] streamId, byte[] ownerId)
        {
            var currentSubscription = SubscriptionsRepository.LoadOwnerSubscription(ownerId);

            if (currentSubscription == null)
                currentSubscription = new Subscription(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()), ownerId);

            currentSubscription.UnSubscribe(streamId);
            SubscriptionsRepository.Save(currentSubscription);

            return this.Accepted(currentSubscription.Id);
        }
    }
}
