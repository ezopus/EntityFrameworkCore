﻿using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Country
{
    public Country()
    {
        Towns = new HashSet<Town>();
    }
    [Key]
    public int CountryId { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<Town> Towns { get; set; }
}
