using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.SellerNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.SellerAddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        public string Website { get; set; }

        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();
    }
}
