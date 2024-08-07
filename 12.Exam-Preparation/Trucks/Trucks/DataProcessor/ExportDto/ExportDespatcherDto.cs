﻿using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class ExportDespatcherDto
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string Name { get; set; } = null!;

        [XmlArray("Trucks")]
        [XmlArrayItem("Truck")]
        public ExportTruckDto[] ExportTruckDtos { get; set; }
    }
}
