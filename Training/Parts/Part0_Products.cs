using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Api.Client;
using commercetools.Api.Models.Customers;
using commercetools.Api.Models.Products;
using commercetools.Api.Models.Categories;
using commercetools.Api.Models.Common;
using commercetools.Api.Models.ProductTypes;
using commercetools.Base.Client;

namespace Training
{
    public class Part0 : IPart
    {
        private readonly IClient _client;

        public Part0(IClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {
            var rand = Settings.RandomInt();
            
            // Create a category
            var categoryDraft = new CategoryDraft
            {
                Name = new LocalizedString {{"en", $"Shoes-{rand}"}},
                Slug = new LocalizedString {{"en", $"shoes-{rand}"}},
                Key = $"shoes-{rand}"
            };
            var category = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Categories()
                .Post(categoryDraft)
                .ExecuteAsync();
            Console.WriteLine($"Created category with key: {category.Key}");

            
            // Create a product type
            var productTypeDraft = new ProductTypeDraft
            {
                Name = $"Boot-{rand}",
                Description = "This is a boot",
                Key = $"boot-{rand}",
                Attributes = new List<IAttributeDefinitionDraft>
                {
                    new AttributeDefinitionDraft
                    {
                        Name = $"color-{rand}",
                        Label = new LocalizedString {{"en", "label"}},
                        IsRequired = false,
                        Type = new AttributeTextType()
                    }
                }
            };
            
            await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .ProductTypes()
                .Post(productTypeDraft)
                .ExecuteAsync();

            var productType = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .ProductTypes()
                .WithKey(productTypeDraft.Key)
                .Get()
                .ExecuteAsync();
            Console.WriteLine($"Created product type with key: {productType.Key}");
            
            
            // Create a product with two variants
            var productDraft = new ProductDraft
            {
                Name = new LocalizedString {{"en", $"James Jodhpur boot {rand}"}},
                Key = $"jodhpurs-boots-{rand}",
                Slug = new LocalizedString{{"en", $"jodhpurs-boots-{rand}"}},
                ProductType = new ProductTypeResourceIdentifier
                {
                    Key = productType.Key
                },
                Categories = new List<ICategoryResourceIdentifier>
                {
                    new CategoryResourceIdentifier
                    {
                        Key = category.Key,
                    }
                },
                Variants = new List<IProductVariantDraft>
                {
                    new ProductVariantDraft
                    {
                        Key = $"James dark brown {rand}",
                        Sku = $"1{rand}"
                    },
                    new ProductVariantDraft
                    {
                        Key = $"James medium brown {rand}",
                        Sku = $"2{rand}"
                    }
                }
            };

            var product = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Products()
                .Post(productDraft)
                .ExecuteAsync();
            Console.WriteLine($"Created product with key: {product.Key}");
        }
    }
}