using Newtonsoft.Json;

namespace ProductShop.DTOs.Import
{
    public class ImportProductDto
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("sellerId")]
        public int SellerId { get; set; }
        [JsonProperty("buyerId")]
        public int? BuyerId { get; set; }
    }
}
