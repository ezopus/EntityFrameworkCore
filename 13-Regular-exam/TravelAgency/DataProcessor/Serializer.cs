using DataProcessor;
using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var xmlHelper = new XmlHelper();

            var exportGuides = context
                .Guides
                .Where(g => g.Language == Enum.Parse<Language>("Spanish"))
                .Select(g => new ExportGuideDto()
                {
                    GuideName = g.FullName,
                    Packages = g.TourPackagesGuides
                        .Select(tp => new ExportTourPackageDto()
                        {
                            PackageName = tp.TourPackage.PackageName,
                            PackageDescription = tp.TourPackage.Description,
                            Price = tp.TourPackage.Price,
                        })
                        .OrderByDescending(tp => tp.Price)
                        .ThenBy(tp => tp.PackageName)
                        .ToArray(),

                })
                .OrderByDescending(g => g.Packages.Length)
                .ThenBy(g => g.GuideName)
                .ToArray();

            return xmlHelper.Serialize(exportGuides, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var exportCustomers = context
                .Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd"),
                        })
                        .ToArray(),
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy(c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(exportCustomers, Formatting.Indented);
        }
    }
}
