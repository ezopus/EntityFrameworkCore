using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Cadastre.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            StringBuilder sb = new StringBuilder();
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Districts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportDistrictDto), xmlRoot);
            XmlHelper xmlHelper = new XmlHelper();

            var importDistrictsDtos = xmlHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");
            ICollection<District> districtEntities = new List<District>();

            foreach (var district in importDistrictsDtos)
            {
                if (!IsValid(district))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dbContext.Districts.FirstOrDefault(d => d.Name == district.Name) != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District districtEntity = new District();
                districtEntity.Name = district.Name;
                districtEntity.PostalCode = district.PostalCode;
                districtEntity.Region = Enum.Parse<Region>(district.Region);

                foreach (var property in district.Properties)
                {
                    if (!IsValid(property))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dbContext.Properties
                            .FirstOrDefault(p => p.PropertyIdentifier == property.PropertyIdentifier) != null
                        || dbContext.Properties
                            .FirstOrDefault(p => p.Address == property.Address) != null)

                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (districtEntity.Properties.FirstOrDefault(p => p.Address == property.Address) != null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property propertyEntity = new Property();
                    propertyEntity.PropertyIdentifier = property.PropertyIdentifier;
                    propertyEntity.Area = property.Area;
                    propertyEntity.DateOfAcquisition = DateTime.ParseExact(property.DateOfAcquisition, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture);
                    propertyEntity.Address = property.Address;
                    propertyEntity.Details = property.Details;
                    propertyEntity.District = districtEntity;

                    districtEntity.Properties.Add(propertyEntity);
                }

                districtEntities.Add(districtEntity);
                sb.AppendLine(String.Format(SuccessfullyImportedDistrict, districtEntity.Name,
                    districtEntity.Properties.Count()));
            }

            Console.WriteLine();
            dbContext.Districts.AddRange(districtEntities);

            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            var citizenDtos = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);

            ICollection<Citizen> citizenEntities = new List<Citizen>();

            foreach (var citizenDto in citizenDtos)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var status = Enum.TryParse(citizenDto.MaritalStatus, true, out MaritalStatus result);

                if (!status)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizen = new Citizen();
                citizen.FirstName = citizenDto.FirstName;
                citizen.LastName = citizenDto.LastName;
                citizen.BirthDate =
                    DateTime.ParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                citizen.MaritalStatus = Enum.Parse<MaritalStatus>(citizenDto.MaritalStatus);

                if (citizenDto.Properties != null)

                {
                    foreach (var property in citizenDto.Properties)
                    {
                        var citizenProperty = dbContext.Properties.Find(property);

                        if (citizenProperty == null)
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }

                        citizen.PropertiesCitizens.Add(new PropertyCitizen()
                        {
                            PropertyId = property,
                            Citizen = citizen,
                        });
                    }
                }

                citizenEntities.Add(citizen);

                sb.AppendLine(String.Format(SuccessfullyImportedCitizen,
                    citizen.FirstName, citizen.LastName,
                    citizen.PropertiesCitizens.Count()));
            }

            dbContext.Citizens.AddRange(citizenEntities);

            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
