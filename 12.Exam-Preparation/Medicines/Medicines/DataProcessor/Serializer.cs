using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;

namespace Medicines.DataProcessor
{
    using Medicines.Data;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var dateParsed = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var exportPatients = context
                .Patients
                .Where(p => p.PatientsMedicines
                    .Any(m => m.Medicine.ProductionDate >= dateParsed))
                .Select(p => new ExportPatientWithMedicineDto()
                {
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Gender = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                        .Where(m => m.Medicine.ProductionDate >= dateParsed)
                        .Select(m => m.Medicine)
                        .OrderByDescending(m => m.ExpiryDate)
                        .ThenBy(m => m.Price)
                        .Select(m => new ExportMedicineDto()
                        {
                            Name = m.Name,
                            Price = m.Price.ToString("f2"),
                            Category = m.Category.ToString().ToLower(),
                            Producer = m.Producer,
                            ExpiryDate = m.ExpiryDate.ToString("yyyy-MM-dd"),
                        })
                        .ToArray(),

                })
                .OrderByDescending(m => m.Medicines.Length)
                .ThenBy(p => p.Name)
                .ToArray();

            return xmlHelper.Serialize(exportPatients, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var output = context
                .Medicines
                .Where(m => m.Category == (Category)medicineCategory
                            && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(p => p.Name)
                .Select(m => new
                {
                    m.Name,
                    Price = m.Price.ToString("f2"),
                    Pharmacy = new
                    {
                        m.Pharmacy.Name,
                        m.Pharmacy.PhoneNumber
                    }
                })
                .ToArray();

            return JsonConvert.SerializeObject(output, Formatting.Indented);
        }
    }
}
