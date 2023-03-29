namespace SoftJail.DataProcessor.ImportDto
{
    using SoftJail.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DepartmentImportDto
    {
        [Required]
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }

        public CellImportDto[] Cells { get; set; }
    }
}