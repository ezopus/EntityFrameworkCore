using Footballers.Shared;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.CoachNameMinLength)]
        [MaxLength(GlobalConstants.CoachNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        [XmlArrayItem("Footballer")]
        public ImportFootballerDto[] Footballers { get; set; }
    }
}
