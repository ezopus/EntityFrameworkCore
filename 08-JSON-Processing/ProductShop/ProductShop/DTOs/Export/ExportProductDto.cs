using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    public class ExportProductDto
    {
        public ExportProductDto(Product product)
        {
            Name = product.Name;
            Price = product.Price;
            SellerFullName = $"{product.Seller.FirstName} {product.Seller.LastName}";
        }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("seller")]
        public string SellerFullName { get; set; }
    }
}
