using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ImportDto;
using DataProcessor;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor
{
    using Artillery.Data;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();

            var importCountriesDtos = xmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            ICollection<Country> countries = new HashSet<Country>();

            foreach (var countryDto in importCountriesDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize,
                };

                countries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();

            var importManufacturersDtos = xmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            ICollection<Manufacturer> manufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturerDto in importManufacturersDtos)
            {
                var isManufacturerAdded =
                    manufacturers.FirstOrDefault(m => m.ManufacturerName.ToLower() == manufacturerDto.ManufacturerName.ToLower());

                if (!IsValid(manufacturerDto) || isManufacturerAdded != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded,
                };

                manufacturers.Add(manufacturer);

                var manufacturerCountry = manufacturer.Founded.Split(", ").ToArray();
                var last = manufacturerCountry.Skip(Math.Max(0, manufacturerCountry.Count() - 2)).ToArray();
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, string.Join(", ", last)));
            }

            context.Manufacturers.AddRange(manufacturers);

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();

            var importShellDtos = xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            ICollection<Shell> shells = new HashSet<Shell>();

            foreach (var shellDto in importShellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new()
                {
                    ShellWeight = shellDto.Weight,
                    Caliber = shellDto.Caliber,
                };

                shells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new();

            var importGunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            ICollection<Gun> guns = new HashSet<Gun>();

            foreach (var gunDto in importGunDtos)
            {

                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool hasGunType = Enum.IsDefined(typeof(GunType), gunDto.GunType);

                if (!hasGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    Range = gunDto.Range,
                    GunType = Enum.Parse<GunType>(gunDto.GunType),
                    ShellId = gunDto.ShellId,
                };

                if (gunDto.NumberBuild.HasValue)
                {
                    gun.NumberBuild = gunDto.NumberBuild.Value;
                }


                if (gunDto.Countries.Length > 0)
                {
                    foreach (var countryId in gunDto.Countries.Distinct())
                    {
                        gun.CountriesGuns.Add(new CountryGun()
                        {
                            Gun = gun,
                            CountryId = countryId.Id,
                        });
                    }
                }


                guns.Add(gun);
                sb.AppendLine(String.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight,
                    gun.BarrelLength));
            }

            context.Guns.AddRange(guns);

            context.SaveChanges();

            return sb.ToString().Trim();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}