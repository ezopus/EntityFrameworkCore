using System.Xml.Serialization;

namespace Invoices.DataProcessor.ExportDto
{
    [XmlType("Client")]
    public class ExportClientDto
    {
        [XmlAttribute]
        public int InvoicesCount { get; set; }

        [XmlElement]
        public string ClientName { get; set; }
        [XmlElement]
        public string VatNumber { get; set; }

        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public ExportInvoiceDto[] exportInvoicesDtos { get; set; }

    }
}
