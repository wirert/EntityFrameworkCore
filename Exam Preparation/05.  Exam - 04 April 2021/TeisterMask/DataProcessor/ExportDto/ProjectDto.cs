namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectDto
    {
        [XmlAttribute]
        public int TasksCount { get; set; }

        [XmlElement]
        public string ProjectName { get; set; }

        [XmlElement]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TaskDto[] Tasks { get; set; }
    }
}
