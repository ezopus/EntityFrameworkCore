using System.Xml;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class ExportSalesWithDiscountDto
    {
        [XmlElement("car")]
        public ExportSaleCarDto SaleCar { get; set; } = null!;

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public double PriceWithDiscount { get; set; }
    }
}
