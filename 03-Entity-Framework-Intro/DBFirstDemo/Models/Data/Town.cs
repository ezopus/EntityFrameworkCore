using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBFirstDemo.Models.Data
{
    public partial class Town
    {
        public Town()
        {
            Addresses = new HashSet<Address>();
        }

        [Key]
        [Column("TownID")]
        public int TownId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        public int? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        [InverseProperty("Town")]
        public virtual ICollection<Address> Addresses { get; set; }


    }
}
