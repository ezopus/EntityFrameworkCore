using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export
{
    public class ExportSoldProductDto
    {
        public ExportSoldProductDto(User user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.SoldProducts = user.ProductsSold.Select(p => new SoldProductDto(p));
        }
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("soldProducts")]
        public IEnumerable<SoldProductDto> SoldProducts { get; set; }
    }
}
