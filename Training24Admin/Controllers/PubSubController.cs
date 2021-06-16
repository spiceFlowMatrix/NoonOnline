using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Api.Gax.ResourceNames;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Cloud.Storage.V1;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PubSubController : ControllerBase
    {
        private IHostingEnvironment hostingEnvironment;

        public PubSubController(
            IHostingEnvironment hostingEnvironment
        )
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("PullNotification")]
        public async Task<IActionResult> NotificationAsync()
        {
            try
            {
                PublisherServiceApiClient publisherService = await PublisherServiceApiClient.CreateAsync();
                TopicName topicName = new TopicName("training24-197210", "Image");
                Topic topic = publisherService.GetTopic(topicName);

                SubscriberServiceApiClient subscriberService = await SubscriberServiceApiClient.CreateAsync();
                SubscriptionName subscriptionName = new SubscriptionName("training24-197210", "MyImage");
                Subscription subscription = subscriberService.GetSubscription(subscriptionName);

                SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscription.SubscriptionName);
                List<PubsubMessage> receivedMessages = new List<PubsubMessage>();

                await subscriber.StartAsync((msg, cancellationToken) =>
                {
                    receivedMessages.Add(msg);
                    Console.WriteLine($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                    Console.WriteLine($"Text: '{msg.Data.ToStringUtf8()}'");
                    subscriber.StopAsync(TimeSpan.FromSeconds(15));
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }
}