using Footballers.Shared;
using System.ComponentModel.DataAnnotations;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MinLength(GlobalConstants.TeamNameMinLength)]
        [MaxLength(GlobalConstants.TeamNameMaxLength)]
        [RegularExpression(GlobalConstants.TeamNameAllowedSymbols)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.TeamNationalityMinLength)]
        [MaxLength(GlobalConstants.TeamNationalityMaxLength)]
        public string Nationality { get; set; } = null!;

        [Required]
        public string Trophies { get; set; } = null!;

        public int[] Footballers { get; set; }
    }
}
