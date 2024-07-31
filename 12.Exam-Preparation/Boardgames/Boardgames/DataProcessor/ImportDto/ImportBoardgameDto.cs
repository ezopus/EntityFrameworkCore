using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(ValidationConstants.BoardgameNameMinLength)]
        [MaxLength(ValidationConstants.BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [XmlElement("Rating")]
        [Range(ValidationConstants.BoardgameRatingMinRange, ValidationConstants.BoardgameRatingMaxRange)]
        public double Rating { get; set; }

        [Required]
        [XmlElement("YearPublished")]
        [Range(ValidationConstants.BoardgameYearMinRange, ValidationConstants.BoardgameYearMaxRange)]
        public int YearPublished { get; set; }

        [Required]
        [Range(ValidationConstants.BoardgameCategoryTypeMinRange, ValidationConstants.BoardgameCategoryTypeMaxRange)]
        public int CategoryType { get; set; }

        [Required]
        public string Mechanics { get; set; } = null!;

    }
}
