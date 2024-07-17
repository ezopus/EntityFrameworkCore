using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            var properties = dbContext
                .Properties
                .Where(p => p.DateOfAcquisition >= new DateTime(2000, 1, 1))
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new
                {
                    p.PropertyIdentifier,
                    p.Area,
                    p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                        .OrderBy(c => c.Citizen.LastName)
                        .Select(pc => new
                        {
                            pc.Citizen.LastName,
                            MaritalStatus = pc.Citizen.MaritalStatus.ToString(),
                        })
                        .ToArray(),
                })
                .ToArray();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext
                .Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .ToArray()
                .Select(p => new ExportPropertyDto()
                {
                    PostalCode = p.District.PostalCode,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    PropertyIdentifier = p.PropertyIdentifier,
                })
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            var result = xmlHelper.Serialize(properties, "Properties");

            return result.TrimEnd();
        }
    }
}
