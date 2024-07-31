using Medicines.Shared;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(ValidationConstants.PatientNameMinLength)]
        [MaxLength(ValidationConstants.PatientNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.PatientAgeGroupMinRange, ValidationConstants.PatientAgeGroupMaxRange)]
        public int AgeGroup { get; set; }

        [Required]
        [Range(ValidationConstants.PatientGenderMinRange, ValidationConstants.PatientGenderMaxRange)]
        public int Gender { get; set; }

        public int[] Medicines { get; set; }

        //{
        //    "FullName": "Ivan Petrov",
        //    "AgeGroup": "1",
        //    "Gender": "0",
        //    "Medicines": [
        //    15,
        //    23
        //        ]
        //},
    }
}
