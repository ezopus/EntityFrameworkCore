using Invoices.Common;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(GlobalConstants.InvoiceNumberMinLength, GlobalConstants.InvoiceNumberMaxLength)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; } = null!;

        [Required]
        public string DueDate { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(GlobalConstants.CurrencyTypeMinRange, GlobalConstants.CurrencyTypeMaxRange)]
        public int CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}
