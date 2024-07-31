using System.ComponentModel.DataAnnotations.Schema;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.CategoryProducts = new HashSet<CategoryProduct>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int SellerId { get; set; }
        public virtual User Seller { get; set; } = null!;

        [ForeignKey(nameof(Buyer))]
        public int? BuyerId { get; set; }
        public virtual User? Buyer { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
    }
}