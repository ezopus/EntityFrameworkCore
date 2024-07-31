using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext dbContext = new CarDealerContext();

            //dbContext.Database.EnsureDeleted();

            //dbContext.Database.EnsureCreated();


            ////09. Import suppliers
            //string inputJsonSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");

            //Console.WriteLine(ImportSuppliers(dbContext, inputJsonSuppliers));

            ////10. Import parts
            //string inputJsonParts = File.ReadAllText("../../../Datasets/parts.json");

            //Console.WriteLine(ImportParts(dbContext, inputJsonParts));

            ////11. Import cars
            //string inputJsonCars = File.ReadAllText("../../../Datasets/cars.json");

            //Console.WriteLine(ImportCars(dbContext, inputJsonCars));

            ////12. Import customers
            //string inputJsonCustomers = File.ReadAllText("../../../Datasets/customers.json");

            //Console.WriteLine(ImportCustomers(dbContext, inputJsonCustomers));

            ////13. Import sales
            //string inputJsonSales = File.ReadAllText("../../../Datasets/sales.json");

            //Console.WriteLine(ImportSales(dbContext, inputJsonSales));

            //14. Export ordered customers
            //Console.WriteLine(GetOrderedCustomers(dbContext));

            //15. Export cars from Make Toyota
            //Console.WriteLine(GetCarsFromMakeToyota(dbContext));

            //16. Export local suppliers
            //Console.WriteLine(GetLocalSuppliers(dbContext));

            //17. Export Cars With Their List Of Parts 
            //Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));

            //18. Export Total Sales By Customer 
            //Console.WriteLine(GetTotalSalesByCustomer(dbContext));

            //19. Export Sales with Applied Discount
            Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ImportSupplierDto[] supplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var s in supplierDtos)
            {
                var supplier = new Supplier()
                {
                    Name = s.Name,
                    IsImporter = s.IsImporter,
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ImportPartDto[] partsDto = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson)!;

            ICollection<Part> parts = new HashSet<Part>();

            foreach (var partDto in partsDto)
            {
                if (context.Suppliers.FirstOrDefault(sp => sp.Id == partDto.SupplierId) == null)
                {
                    continue;
                }

                var part = new Part()
                {
                    Name = partDto.Name,
                    Price = partDto.Price,
                    Quantity = partDto.Quantity,
                    SupplierId = partDto.SupplierId,
                };

                parts.Add(part);
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            ICollection<Car> cars = new HashSet<Car>();
            ICollection<PartCar> partsCars = new HashSet<PartCar>();

            foreach (var carDto in importCarDtos)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance,
                };

                foreach (var partId in carDto.partsIds.Distinct())
                {
                    var partCar = new PartCar()
                    {
                        PartId = partId,
                        Car = car,
                    };
                    partsCars.Add(partCar);
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partsCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var importCustomerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (var customerDto in importCustomerDtos)
            {
                var customer = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver,
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var importSalesDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (var saleDto in importSalesDtos)
            {
                var sale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount,
                };
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    c.IsYoungDriver
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new ExportCarDto(c))
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count,
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars.Select(cp => new
                    {
                        cp.Part.Name,
                        Price = cp.Part.Price.ToString("f2"),
                    })
                        .ToArray(),
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.SelectMany(s => s.Car.PartsCars).Sum(pc => pc.Part.Price),
                })
                .AsNoTracking()
                .OrderByDescending(s => s.spentMoney)
                .ThenBy(s => s.boughtCars)
                .ToArray();


            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount =
                        Math.Round(s.Car.PartsCars
                                .Sum(p => p.Part.Price) * (100 - s.Discount) / 100, 2)
                        .ToString("f2"),
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }
    }
}