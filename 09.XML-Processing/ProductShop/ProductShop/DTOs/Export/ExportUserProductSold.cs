﻿using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    public class ExportUserProductSold
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
