using Boardgames.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [Required]
        [XmlElement("FirstName")]
        [MinLength(ValidationConstants.CreatorFirstNameMinLength)]
        [MaxLength(ValidationConstants.CreatorFirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [XmlElement("LastName")]
        [MinLength(ValidationConstants.CreatorLastNameMinLength)]
        [MaxLength(ValidationConstants.CreatorLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        [XmlArrayItem("Boardgame")]
        public ImportBoardgameDto[] ImportBoardgameDtos { get; set; }
    }
}
