﻿using System.ComponentModel.DataAnnotations;
using Trucks.Common;

namespace Trucks.Data.Models;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ClientNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.ClientNationalityMaxLength)]
    public string Nationality { get; set; } = null!;

    [Required]
    public string Type { get; set; } = null!;

    public ICollection<ClientTruck> ClientsTrucks { get; set; } = new HashSet<ClientTruck>();
}

