namespace TravelAgency.Common
{
    public static class ValidationConstants
    {
        public const int CustomerNameMinLength = 4;
        public const int CustomerNameMaxLength = 60;
        public const int CustomerEmailMinLength = 6;
        public const int CustomerEmailMaxLength = 50;
        public const int CustomerPhoneNumber = 13;
        public const string CustomerPhoneNumberRegex = @"^\+\d{12}$";

        public const int GuideFullNameMinLength = 4;
        public const int GuideFullNameMaxLength = 60;


        public const int TourPackageNameMinLength = 2;
        public const int TourPackageNameMaxLength = 40;
        public const int TourPackageDescriptionMaxLength = 200;
    }
}
