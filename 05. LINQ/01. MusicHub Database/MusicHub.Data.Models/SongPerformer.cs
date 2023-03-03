﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;

public class SongPerformer
{
    public int SongId { get; set; }

    [ForeignKey(nameof(SongId))]
    public virtual Song Song { get; set; } = null!;

    public int PerformerId { get; set; }

    [ForeignKey(nameof(PerformerId))]
    public virtual Performer Performer { get; set;} = null!;
}
