using Webshop.Domain.Common;

namespace Webshop.Search.Domain.AggregateRoots
{
    public class SearchProduct : AggregateRoot
    {
        public SearchProduct(string name, string sku, int price, string currency, string description, int categoryId)
        {
            Name = name;
            SKU = sku;
            Price = price;
            Currency = currency;
            Description = description;
            CategoryId = categoryId;
        }

        public SearchProduct() { } // For ORM
        public string Name { get; private set; }
        public string Description { get; set; }
        public string SKU { get; set; }

        /// <summary>
        /// The price is represented in the lowest monetary value. For Euros, this is cents.
        /// </summary>
        public int Price { get; private set; }
        public string Currency { get; private set; }

        /// <summary>
        /// Foreign key to the Category this product belongs to.
        /// </summary>
        public int CategoryId { get; private set; }
    }
}
