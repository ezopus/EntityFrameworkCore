namespace Footballers.Shared
{
    public static class GlobalConstants
    {
        public const int FootballerNameMinLength = 2;
        public const int FootballerNameMaxLength = 40;

        public const int TeamNameMinLength = 3;
        public const int TeamNameMaxLength = 40;
        public const string TeamNameAllowedSymbols = @"[A-Za-z0-9\s\.\-]{3,}$";

        public const int TeamNationalityMinLength = 2;
        public const int TeamNationalityMaxLength = 40;

        public const int CoachNameMinLength = 2;
        public const int CoachNameMaxLength = 40;

    }
}
