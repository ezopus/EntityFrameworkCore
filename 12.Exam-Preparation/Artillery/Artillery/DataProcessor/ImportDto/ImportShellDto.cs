using Artillery.Common;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Required]
        [XmlElement("ShellWeight")]
        [Range(ValidationConstants.ShellMinWeight, ValidationConstants.ShellMaxWeight)]
        public double Weight { get; set; }

        [Required]
        [XmlElement("Caliber")]
        [MinLength(ValidationConstants.CaliberMinLength)]
        [MaxLength(ValidationConstants.CaliberMaxLength)]
        public string Caliber { get; set; } = null!;
    }
}
