using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Api.Client;
using commercetools.Api.Models.Customers;
using commercetools.Api.Models.Common;
using commercetools.Api.Models.Types;
using commercetools.Base.Client;

namespace Training
{
    public class Part1 : IPart
    {
        private readonly IClient _client;

        public Part1(IClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            var rand = Settings.RandomInt();

            // Create new custom type
            var typeDraft = new TypeDraft
            {
                Key = $"shoe-size-{rand}",
                Name = new LocalizedString {{"en", $"Shoe size {rand}"}},
                ResourceTypeIds = new List<IResourceTypeId> {IResourceTypeId.Customer},
                FieldDefinitions = new List<IFieldDefinition>
                {
                    new FieldDefinition
                    {
                        Name = $"shoe-size-{rand}",
                        Required = false,
                        Label = new LocalizedString {{"en", "Shoe size of the customer"}},
                        Type = new CustomFieldNumberType()
                    }
                }
            };
            
            var createdType = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Types()
                .Post(typeDraft)
                .ExecuteAsync();
            Console.WriteLine($"New custom type with key: {createdType.Key}");
            
            // Create customer
            var customerDraft = new CustomerDraft
            {
                Email = $"customer-{rand}@example.com",
                Password = "password123",
                Key = $"customer-{rand}",
                Custom = new CustomFieldsDraft
                {
                    Fields = new FieldContainer {{typeDraft.Key, 43}},
                    Type = new TypeResourceIdentifier
                    {
                        Key = $"shoe-size-{rand}"
                    }
                }
            };
            
            var customerSignUp = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Customers()
                .Post(customerDraft)
                .ExecuteAsync();
            var customer = customerSignUp.Customer;
            Console.WriteLine($"Customer Created with shoe size: {customer.Custom.Fields.First().Value}");
        }
    }
}