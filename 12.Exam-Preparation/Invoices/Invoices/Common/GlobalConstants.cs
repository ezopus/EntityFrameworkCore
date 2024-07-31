namespace Invoices.Common
{
    public static class GlobalConstants
    {
        public const int ProductNameMinLength = 9;
        public const int ProductNameMaxLength = 30;

        public const double ProductPriceMinRange = 5.00d;
        public const double ProductPriceMaxRange = 1000.00d;

        public const int StreetNameMinLength = 10;
        public const int StreetNameMaxLength = 20;

        public const int CityMinLength = 5;
        public const int CityMaxLength = 15;

        public const int CountryMinLength = 5;
        public const int CountryMaxLength = 15;

        public const int InvoiceNumberMinLength = 1_000_000_000;
        public const int InvoiceNumberMaxLength = 1_500_000_000;

        public const int CategoryTypeMinRange = 0;
        public const int CategoryTypeMaxRange = 4;

        public const int CurrencyTypeMinRange = 0;
        public const int CurrencyTypeMaxRange = 2;

        public const int ClientNameMinLength = 10;
        public const int ClientNameMaxLength = 25;
        public const int ClientVATMinLength = 10;
        public const int ClientVATMaxLength = 15;
    }
}
