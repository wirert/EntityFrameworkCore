﻿namespace SoftJail.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Message")]
    public class MailExportDto
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
