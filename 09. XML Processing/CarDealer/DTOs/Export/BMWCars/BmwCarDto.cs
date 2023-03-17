﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export.BMWCars
{
    [XmlType("car")]
    public class BmwCarDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance { get; set; }
    }
}
