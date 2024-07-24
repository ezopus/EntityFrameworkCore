namespace Trucks.Common;
public static class ValidationConstants
{
    public const int TruckRegNumberLength = 8;
    public const string TruckRegistrationRegex = @"^[A-Z]{2}\d{4}[A-Z]{2}$";
    public const int TruckVinNumberLength = 17;

    public const int TruckTankMinCapacity = 950;
    public const int TruckTankMaxCapacity = 1420;

    public const int TruckCargoMinCapacity = 5000;
    public const int TruckCargoMaxCapacity = 29000;


    public const int ClientNameMinLength = 3;
    public const int ClientNameMaxLength = 40;

    public const int ClientNationalityMinLength = 2;
    public const int ClientNationalityMaxLength = 40;

    public const int DespatcherNameMinLength = 2;
    public const int DespatcherNameMaxLength = 40;

    public const int CategoryMaxRange = 3;
    public const int MakeMaxRange = 4;
}

