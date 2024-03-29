﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Color
{
    public Color()
    {
        PrimaryKitTeams = new HashSet<Team>();
        SecondaryKitTeams = new HashSet<Team>();
    }

    [Key]
    public int ColorId { get; set; }

    [Required]
    [MaxLength(Constants.ColorNameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [InverseProperty(nameof(Team.PrimaryKitColor))]
    public virtual ICollection<Team> PrimaryKitTeams { get; set; }

    [InverseProperty(nameof(Team.SecondaryKitColor))]
    public virtual ICollection<Team> SecondaryKitTeams { get; set; }
}
