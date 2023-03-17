﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.CarsWithParts
{
    [XmlType("car")]
    public class CarDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; } = null!;

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }

        [XmlArray("parts")]
        public CarPartDto[] PartsCars { get; set; }
    }
}
