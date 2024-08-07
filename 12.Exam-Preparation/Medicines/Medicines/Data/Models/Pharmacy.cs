﻿using Medicines.Shared;
using System.ComponentModel.DataAnnotations;

namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.MedicineNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.PhoneNumberLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public bool IsNonStop { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; } = new HashSet<Medicine>();

        //• Id – integer, Primary Key
        //• Name – text with length[2, 50] (required)
        //• PhoneNumber – text with length 14. (required)
        //◦ All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits: 
        //▪ Example -> (123) 456-7890 
        //• IsNonStop – bool (required)
        //• Medicines - collection of type Medicine
    }
}
