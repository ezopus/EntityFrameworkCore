namespace Cadastre.Shared
{
    public static class GlobalConstants
    {
        //Citizen
        public const int CitizenFirstNameMinLength = 2;
        public const int CitizenFirstNameMaxLength = 30;
        public const int CitizenLastNameMinLength = 2;
        public const int CitizenLastNameMaxLength = 30;

        //Property
        public const int PropertyIdentifierMinLength = 16;
        public const int PropertyIdentifierMaxLength = 20;

        public const int PropertyDetailsMinLength = 5;
        public const int PropertyDetailsMaxLength = 500;

        public const int PropertyAddressMinLength = 5;
        public const int PropertyAddressMaxLength = 200;

        //Properties
        public const int DistrictNameMinLength = 2;
        public const int DistrictNameMaxLength = 80;

        public const int DistrictPostalCodeLength = 8;
        public const string DistrictPostalCodeRegex = @"^[A-Z]{2}-[0-9]{5}$";
    }
}
