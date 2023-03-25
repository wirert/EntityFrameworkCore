namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class TaskImportDto
    {
        [XmlElement("Name")]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [XmlElement("OpenDate", IsNullable = false)]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate", IsNullable = false)]
        [Required]
        public string DueDate { get; set; }

        [XmlElement]
        [Required]
        [Range(0, 3)]
        public int ExecutionType { get; set; }

        [XmlElement]
        [Required]
        [Range(0, 4)]
        public int LabelType { get; set; }
    }
}
