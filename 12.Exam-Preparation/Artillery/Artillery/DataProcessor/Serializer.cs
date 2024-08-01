using Artillery.DataProcessor.ExportDto;
using DataProcessor;

namespace Artillery.DataProcessor
{
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var exportShells = context
                .Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new
                {
                    s.ShellWeight,
                    s.Caliber,
                    Guns = s.Guns
                        .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                        .OrderByDescending(g => g.GunWeight)
                        .ToArray()
                        .Select(g => new
                        {
                            GunType = g.GunType.ToString(),
                            g.GunWeight,
                            g.BarrelLength,
                            Range = g.Range > 3000 ? "Regular range" : "Long-range"
                        })
                        .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(exportShells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            XmlHelper xmlHelper = new();

            var exportGuns = context
                .Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new ExportGunDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Countries = g.CountriesGuns
                        .Where(c => c.Country.ArmySize > 4500000)
                        .OrderBy(c => c.Country.ArmySize)
                        .Select(cg => new ExportCountryDto()
                        {
                            CountryName = cg.Country.CountryName,
                            ArmySize = cg.Country.ArmySize
                        })
                        .ToArray()
                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            return xmlHelper.Serialize(exportGuns, "Guns");
        }
    }
}
