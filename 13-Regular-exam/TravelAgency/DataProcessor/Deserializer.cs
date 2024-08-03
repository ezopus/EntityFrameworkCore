using DataProcessor;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new();

            var importCustomerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (var customerDto in importCustomerDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (customers
                    .Any(c => c.Email.ToLower() == customerDto.Email.ToLower()
                              || c.FullName.ToLower() == customerDto.CustomerFullName.ToLower()
                              || c.PhoneNumber == customerDto.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer customer = new()
                {
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber,
                    FullName = customerDto.CustomerFullName,
                };

                customers.Add(customer);
                sb.AppendLine(String.Format(SuccessfullyImportedCustomer, customer.FullName));
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var importBookingsDto = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);

            ICollection<Booking> bookings = new HashSet<Booking>();

            foreach (var bookingDto in importBookingsDto)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isDateValid = DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate);

                //var customerFound =
                //    context.Customers.FirstOrDefault(c => c.FullName.ToLower() == bookingDto.CustomerName);

                //var tourPackageFound =
                //    context.TourPackages.FirstOrDefault(tp => tp.PackageName.ToLower() == bookingDto.TourPackageName);

                if (!isDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var customerId = context.Customers.Where(c => c.FullName == bookingDto.CustomerName).Select(c => c.Id).FirstOrDefault();

                var packageId = context.TourPackages.Where(tp => tp.PackageName == bookingDto.TourPackageName).Select(tp => tp.Id).FirstOrDefault();

                var booking = new Booking()
                {
                    BookingDate = parsedDate,
                    CustomerId = customerId,
                    TourPackageId = packageId,
                };

                sb.AppendLine(String.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName,
                    booking.BookingDate.ToString("yyyy-MM-dd")));
                bookings.Add(booking);

            }

            context.Bookings.AddRange(bookings);

            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
