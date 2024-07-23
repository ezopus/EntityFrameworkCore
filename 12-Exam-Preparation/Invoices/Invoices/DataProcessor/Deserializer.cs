using DataProcessor;
using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using Invoices.DataProcessor.ImportDto;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            var importClientDtos = xmlHelper.Deserialize<ImportClientDto[]>(xmlString, "Clients");

            ICollection<Client> clients = new HashSet<Client>();

            foreach (var importClientDto in importClientDtos)
            {
                if (!IsValid(importClientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = importClientDto.Name,
                    NumberVat = importClientDto.NumberVat,
                };

                foreach (var addressDto in importClientDto.ImportAddressDtos)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage); continue;
                    }

                    var address = new Address()
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        City = addressDto.CityName,
                        Country = addressDto.CountryName,
                        PostCode = addressDto.PostCode,
                    };

                    client.Addresses.Add(address);
                }

                clients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClients, client.Name));
            }

            context.Clients.AddRange(clients);

            context.SaveChanges();

            return sb.ToString().Trim();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();

            var importInvoiceDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);

            ICollection<Invoice> invoices = new HashSet<Invoice>();

            foreach (var importInvoiceDto in importInvoiceDtos)
            {
                if (!IsValid(importInvoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isIssueDateValid = DateTime.TryParse(importInvoiceDto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime issueDateParsed);
                var isDueDateValid = DateTime.TryParse(importInvoiceDto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateParsed);

                var client = context.Clients.FirstOrDefault(c => c.Id == importInvoiceDto.ClientId);

                if (!isIssueDateValid
                    || !isDueDateValid
                    || dueDateParsed < issueDateParsed
                    || importInvoiceDto.Amount < 0
                    || client == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = new()
                {
                    Number = importInvoiceDto.Number,
                    IssueDate = issueDateParsed,
                    DueDate = dueDateParsed,
                    Amount = importInvoiceDto.Amount,
                    CurrencyType = (CurrencyType)importInvoiceDto.CurrencyType,
                    ClientId = importInvoiceDto.ClientId,
                };
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoice.Number));

                invoices.Add(invoice);
            }

            context.Invoices.AddRange(invoices);

            context.SaveChanges();
            return sb.ToString().Trim();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();

            var importProductsDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            ICollection<Product> products = new HashSet<Product>();
            foreach (var productDto in importProductsDtos)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var product = new Product()
                {
                    Name = productDto.Name,
                    CategoryType = (CategoryType)productDto.CategoryType,
                    Price = (decimal)productDto.Price
                };

                foreach (var clientId in productDto.Clients.Distinct())
                {
                    var client = context.Clients.Find(clientId);

                    if (client == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var productClient = new ProductClient()
                    {
                        Client = client,
                        Product = product,
                    };

                    product.ProductsClients.Add(productClient);
                }
                products.Add(product);

                sb.AppendLine(String.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count));
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
