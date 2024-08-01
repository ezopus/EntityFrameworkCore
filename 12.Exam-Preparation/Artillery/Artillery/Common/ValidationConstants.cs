namespace Artillery.Common
{
    public class ValidationConstants
    {
        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 60;
        public const int CountryNameMinArmy = 50_000;
        public const int CountryNameMaxArmy = 100_000_000;

        public const int ManufacturerNameMinLength = 4;
        public const int ManufacturerNameMaxLength = 40;
        public const int ManufacturerMinFounded = 10;
        public const int ManufacturerMaxFounded = 100;

        public const double ShellMinWeight = 2.0;
        public const double ShellMaxWeight = 1680.0;
        public const int CaliberMinLength = 4;
        public const int CaliberMaxLength = 30;

        public const int GunWeightMin = 100;
        public const int GunWeightMax = 1_350_000;
        public const double BarrelMinLength = 2.00;
        public const double BarrelMaxLength = 35.00;
        public const int GunMinRange = 1;
        public const int GunMaxRange = 100_000;
    }
}
