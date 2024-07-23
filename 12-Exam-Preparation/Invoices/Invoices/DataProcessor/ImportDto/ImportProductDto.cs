using Invoices.Common;
using System.ComponentModel.DataAnnotations;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductDto
    {
        [Required]
        [MinLength(GlobalConstants.ProductNameMinLength)]
        [MaxLength(GlobalConstants.ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(GlobalConstants.ProductPriceMinRange, GlobalConstants.ProductPriceMaxRange)]
        public double Price { get; set; }

        [Required]
        [Range(GlobalConstants.CategoryTypeMinRange, GlobalConstants.CategoryTypeMaxRange)]
        public int CategoryType { get; set; }

        public int[] Clients { get; set; } = null!;
    }
}
