﻿namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class CellImportDto
    {
        [Required]
        [Range(1, 1000)]
        public int CellNumber { get; set; }

        [Required]
        public bool HasWindow { get; set; }
    }
}
