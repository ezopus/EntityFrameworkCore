using DataProcessor;
using Invoices.DataProcessor.ExportDto;
using Newtonsoft.Json;
using System.Globalization;

namespace Invoices.DataProcessor
{
    using Invoices.Data;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var exportClientsDtos = context
                .Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate >= date))
                .Select(c => new ExportClientDto()
                {
                    ClientName = c.Name,
                    InvoicesCount = c.Invoices.Count(),
                    VatNumber = c.NumberVat,
                    exportInvoicesDtos = c.Invoices
                        .OrderBy(i => i.IssueDate)
                        .ThenByDescending(i => i.DueDate)
                        .Select(i => new ExportInvoiceDto()
                        {
                            Currency = i.CurrencyType.ToString(),
                            InvoiceAmount = i.Amount.ToString("0.##"),
                            DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            InvoiceNumber = i.Number
                        })
                        .ToArray(),
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray();

            return xmlHelper.Serialize(exportClientsDtos, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var exportProducts = context
                .Products
                .Where(p => p.ProductsClients.Any(c => c.Client.Name.Length >= nameLength))
                //.Take(5)
                .ToArray()
                .Select(p => new
                {
                    p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                        .Where(c => c.Client.Name.Length >= nameLength)
                        .ToArray()
                        .OrderBy(c => c.Client.Name)
                        .Select(c => new
                        {
                            Name = c.Client.Name,
                            NumberVat = c.Client.NumberVat,
                        })
                        .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(exportProducts, Formatting.Indented);
        }
    }
}