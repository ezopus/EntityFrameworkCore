using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    public class ExportUserProductSoldWrapper
    {
        [XmlElement("count")]
        public int SoldProductsCount { get; set; }

        [XmlArray("products")]
        [XmlArrayItem("Product")]
        public ExportUserProductSold[] Products { get; set; } = null!;

    }
}
