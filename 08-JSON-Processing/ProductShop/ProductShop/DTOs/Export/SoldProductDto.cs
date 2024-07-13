using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    public class SoldProductDto
    {
        public SoldProductDto(Product product)
        {
            this.Name = product.Name;
            this.Price = product.Price;
            this.BuyerFirstName = product.Buyer.FirstName;
            this.BuyerLastName = product.Buyer.LastName;
        }
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("buyerFirstName")]
        public string BuyerFirstName { get; set; } = null!;
        [JsonProperty("buyerLastName")]
        public string BuyerLastName { get; set; } = null!;
    }
}
