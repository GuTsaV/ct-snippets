using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Api.Client;
using commercetools.Api.Models.Subscriptions;
using commercetools.Base.Client;

namespace Training
{
    public class Part3 : IPart
    {
        private readonly IClient _client;

        public Part3(IClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            var rand = Settings.RandomInt();
            var destination = new GoogleCloudPubSubDestination()
            {
                Type = "GoogleCloudPubSub",
                ProjectId = "ct-subscription",
                Topic = "ct-new-product"
            };
            
            var subscriptionDraft = new SubscriptionDraft
            {
                Key = $"subscription-{rand}",
                Destination = destination,
                Messages = new List<IMessageSubscription>
                {
                    new MessageSubscription
                    {
                        ResourceTypeId = "product",
                        Types = new List<string>{"ProductCreated"}
                    }
                }
            };

            var subscription = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Subscriptions()
                .Post(subscriptionDraft)
                .ExecuteAsync();
            Console.WriteLine($"New subscription with id: {subscription.Id}");
        }
    }
}