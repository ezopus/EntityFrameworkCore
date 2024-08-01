using Artillery.Common;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {
        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [Range(ValidationConstants.GunWeightMin, ValidationConstants.GunWeightMax)]
        public int GunWeight { get; set; }

        [Required]
        [Range(ValidationConstants.BarrelMinLength, ValidationConstants.BarrelMaxLength)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        [Range(ValidationConstants.GunMinRange, ValidationConstants.GunMaxRange)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; } = null!;

        [Required]
        public int ShellId { get; set; }

        public ImportGunCountryDto[] Countries { get; set; } = null!;



    }
}
