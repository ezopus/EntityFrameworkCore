using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var importPatientDtos = JsonConvert.DeserializeObject<ImportPatientDto[]>(jsonString);

            ICollection<Patient> patients = new HashSet<Patient>();

            foreach (var patientDto in importPatientDtos)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                };

                foreach (var medicineId in patientDto.Medicines)
                {
                    if (patient.PatientsMedicines.FirstOrDefault(m => m.MedicineId == medicineId) != null
                        || context.Medicines.Find(medicineId) == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = context.Medicines.Find(medicineId)!;

                    patient.PatientsMedicines.Add(new PatientMedicine()
                    {
                        MedicineId = medicineId,
                        Patient = patient
                    });
                }

                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
                patients.Add(patient);
            }

            context.Patients.AddRange(patients);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper xmlHelper = new XmlHelper();

            var importPharmaciesDtos = xmlHelper.Deserialize<ImportPharmacyDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> pharmacies = new List<Pharmacy>();

            foreach (var importPharmacyDto in importPharmaciesDtos)
            {
                if (!IsValid(importPharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var openCheck = Boolean.TryParse(importPharmacyDto.IsNonStop, out bool isOpen);

                if (!openCheck)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = importPharmacyDto.Name,
                    IsNonStop = Boolean.Parse(importPharmacyDto.IsNonStop),
                    PhoneNumber = importPharmacyDto.PhoneNumber,
                };


                foreach (var medicineDto in importPharmacyDto.Medicines)
                {

                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var productionDate = DateTime.ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var expirationDate = DateTime.ParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);

                    if (expirationDate <= productionDate ||
                        pharmacy.Medicines
                            .FirstOrDefault(m => m.Name == medicineDto.Name
                                                 && m.Producer == medicineDto.Producer) != null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var medicine = new Medicine()
                    {
                        Name = medicineDto.Name,
                        Price = decimal.Parse(medicineDto.Price.ToString()),
                        Category = (Category)medicineDto.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expirationDate,
                        Producer = medicineDto.Producer,
                        Pharmacy = pharmacy,
                    };
                    pharmacy.Medicines.Add(medicine);
                }

                pharmacies.Add(pharmacy);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count()));
            }

            context.Pharmacies.AddRange(pharmacies);

            context.SaveChanges();

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
