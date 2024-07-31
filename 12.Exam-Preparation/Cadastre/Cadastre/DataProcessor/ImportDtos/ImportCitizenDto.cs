using Cadastre.Shared;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {
        [Required]
        [MinLength(GlobalConstants.CitizenFirstNameMinLength)]
        [MaxLength(GlobalConstants.CitizenLastNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(GlobalConstants.CitizenLastNameMinLength)]
        [MaxLength(GlobalConstants.CitizenLastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public string BirthDate { get; set; } = null!;

        [Required]
        public string MaritalStatus { get; set; } = null!;

        public int[]? Properties { get; set; }

        //{
        //    "FirstName": "Ivan",
        //    "LastName": "Georgiev",
        //    "BirthDate": "12-05-1980",
        //    "MaritalStatus": "Married",
        //    "Properties": [ 17, 29 ]
        //}
    }
}
