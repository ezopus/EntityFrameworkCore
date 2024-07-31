using DataProcessor;
using Newtonsoft.Json;
using Trucks.DataProcessor.ExportDto;

namespace Trucks.DataProcessor
{
    using Data;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new();

            var exportDespatchers = context
                .Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new ExportDespatcherDto()
                {
                    Name = d.Name,
                    TrucksCount = d.Trucks.Count,
                    ExportTruckDtos = d.Trucks
                        .OrderBy(t => t.RegistrationNumber)
                        .Select(t => new ExportTruckDto()
                        {
                            RegistrationNumber = t.RegistrationNumber,
                            Make = t.MakeType.ToString(),
                        })
                        .ToArray(),
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.Name)
                .ToArray();

            return xmlHelper.Serialize(exportDespatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var exportClients = context
                .Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                //.ToArray()
                .Select(c => new
                {
                    c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(t => t.Truck.TankCapacity >= capacity)
                        .OrderBy(t => t.Truck.MakeType)
                        .ThenByDescending(t => t.Truck.CargoCapacity)
                        .Select(t => new
                        {
                            TruckRegistrationNumber = t.Truck.RegistrationNumber,
                            t.Truck.VinNumber,
                            t.Truck.TankCapacity,
                            t.Truck.CargoCapacity,
                            CategoryType = t.Truck.CategoryType.ToString(),
                            MakeType = t.Truck.MakeType.ToString(),
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Length)
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(exportClients, Formatting.Indented);
        }
    }
}
