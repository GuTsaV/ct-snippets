using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Api.Client;
using commercetools.Api.Models.Extensions;
using commercetools.Base.Client;

namespace Training
{
    public class Part2 : IPart
    {
        private readonly IClient _client;

        public Part2(IClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            var rand = Settings.RandomInt();
            
            // Create an Extension trigger (on Order Create)
            var trigger = new ExtensionTrigger
            {
                ResourceTypeId = IExtensionResourceTypeId.Order,
                Actions = new List<IExtensionAction> { IExtensionAction.Create }
            };
            
            // Create destination
            var destination = new ExtensionHttpDestination()
            {
                Type = "HTTP",
                Url = "http://c2119318f4fb.ngrok.io"
            };

            // Create extensionDraft
            var extensionDraft = new ExtensionDraft
            {
                Key = $"orderChecker-{rand}",
                Destination = destination,
                Triggers = new List<IExtensionTrigger> { trigger }
            };
            
            var extension = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Extensions()
                .Post(extensionDraft)
                .ExecuteAsync();
            Console.WriteLine($"Extension created with id: {extension.Id}");
        }
    }
}