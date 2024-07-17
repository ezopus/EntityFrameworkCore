namespace Medicines.Shared
{
    public static class ValidationConstants
    {
        //Pharmacy
        public const int PharmacyNameMinLength = 2;
        public const int PharmacyNameMaxLength = 50;

        public const int PhoneNumberLength = 14;
        public const string PhoneNumberRegex = @"^\([0-9]{3}\)\s[0-9]{3}-[0-9]{4}$";

        //Medicine
        public const int MedicineNameMinLength = 3;
        public const int MedicineNameMaxLength = 150;

        public const double MedicinePriceRangeMin = 0.01;
        public const double MedicinePriceRangeMax = 1000.00;

        public const int MedicineProducerMinLength = 3;
        public const int MedicineProducerMaxLength = 100;

        public const int MedicineCategoryMinRange = 0;
        public const int MedicineCategoryMaxRange = 4;

        //Patient
        public const int PatientNameMinLength = 5;
        public const int PatientNameMaxLength = 100;

        public const int PatientAgeGroupMinRange = 0;
        public const int PatientAgeGroupMaxRange = 2;

        public const int PatientGenderMinRange = 0;
        public const int PatientGenderMaxRange = 1;
    }
}
