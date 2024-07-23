namespace CarDealer;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using DTOs.Export;
using DTOs.Import;
using Microsoft.EntityFrameworkCore;
using Models;
using Utilities;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext dbContext = new CarDealerContext();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        //string inputCarsXml = File.ReadAllText("../../../Datasets/cars.xml");
        //string inputCustomersXml = File.ReadAllText("../../../Datasets/customers.xml");
        //string inputPartsXml = File.ReadAllText("../../../Datasets/parts.xml");
        //string inputSalesXml = File.ReadAllText("../../../Datasets/sales.xml");
        //string inputSuppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");

        //9. Import Suppliers
        //Console.WriteLine(ImportSuppliers(dbContext, inputSuppliersXml));

        //10. Import Parts
        //Console.WriteLine(ImportParts(dbContext, inputPartsXml));

        //11. Import Cars
        //Console.WriteLine(ImportCars(dbContext, inputCarsXml));

        //12. Import Customers
        //Console.WriteLine(ImportCustomers(dbContext, inputCustomersXml));

        //13. Import Sales
        //Console.WriteLine(ImportSales(dbContext, inputSalesXml));

        //14. Export Cars With Distance
        //Console.WriteLine(GetCarsWithDistance(dbContext));

        //15. Export Cars from Make BMW
        //Console.WriteLine(GetCarsFromMakeBmw(dbContext));

        //16. Export Local Suppliers
        //Console.WriteLine(GetLocalSuppliers(dbContext));

        //17. Export Cars with Their List of Parts
        //Console.WriteLine(GetCarsWithTheirListOfParts(dbContext));

        //18. Export Total Sales by Customer
        //Console.WriteLine(GetTotalSalesByCustomer(dbContext));

        //19. Export Sales with Applied Discount
        Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));

    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportSupplierDto[] supplierDtos
            = xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

        ICollection<Supplier> suppliers = new HashSet<Supplier>();

        foreach (var supplierDto in supplierDtos)
        {
            if (string.IsNullOrEmpty(supplierDto.Name))
            {
                continue;
            }

            ////Manual mapping
            //Supplier supplier = new Supplier()
            //{
            //    Name = s.Name,
            //    IsImporter = s.IsImporter,
            //};

            Supplier supplier = mapper.Map<Supplier>(supplierDto);

            suppliers.Add(supplier);
        }

        context.Suppliers.AddRange(suppliers);

        context.SaveChanges();

        return $"Successfully imported {suppliers.Count}";
    }

    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();
        ImportPartDto[] partsDtos = xmlHelper.Deserialize<ImportPartDto[]>(inputXml, "Parts");

        ICollection<Part> parts = new HashSet<Part>();

        foreach (var partDto in partsDtos)
        {
            if (string.IsNullOrEmpty(partDto.Name)
                || context.Suppliers.FirstOrDefault(s => s.Id == partDto.SupplierId) == null)
            {
                continue;
            }
            Part part = mapper.Map<Part>(partDto);

            parts.Add(part);
        }

        context.Parts.AddRange(parts);
        context.SaveChanges();

        return $"Successfully imported {parts.Count}";
    }

    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportCarDto[] carDtos
            = xmlHelper.Deserialize<ImportCarDto[]>(inputXml, "Cars");

        ICollection<Car> cars = new HashSet<Car>();
        ICollection<PartCar> carParts = new HashSet<PartCar>();

        foreach (var carDto in carDtos)
        {
            if (String.IsNullOrEmpty(carDto.Make)
                || string.IsNullOrEmpty(carDto.Model))
            {
                continue;
            }

            Car car = mapper.Map<Car>(carDto);

            foreach (var partDto in carDto.Parts.DistinctBy(p => p.PartId))
            {
                if (!context.Parts.Any(p => p.Id == partDto.PartId))
                {
                    continue;
                }

                PartCar carPart = new PartCar()
                {
                    Car = car,
                    PartId = partDto.PartId,
                };
                carParts.Add(carPart);
            }
            cars.Add(car);
        }

        context.Cars.AddRange(cars);
        context.PartsCars.AddRange(carParts);

        context.SaveChanges();
        return $"Successfully imported {cars.Count}";
    }

    public static string ImportCustomers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

        ICollection<Customer> customers = new HashSet<Customer>();

        foreach (var customerDto in customerDtos)
        {
            if (String.IsNullOrEmpty(customerDto.Name) ||
                String.IsNullOrEmpty(customerDto.BirthDate))
            {
                continue;
            }

            Customer customer = mapper.Map<Customer>(customerDto);

            customers.Add(customer);
        }

        context.Customers.AddRange(customers);

        context.SaveChanges();

        return $"Successfully imported {customers.Count}";
    }

    public static string ImportSales(CarDealerContext context, string inputXml)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportSaleDto[] saleDtos = xmlHelper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

        ICollection<Sale> sales = new HashSet<Sale>();

        //optimization - use only one query to db to get all car id's, so
        //we don't need to make individual queries in foreach below
        ICollection<int> dbCarIds = context.Cars.Select(c => c.Id).ToArray();

        foreach (var saleDto in saleDtos)
        {
            if (saleDto.CarId == null
                || dbCarIds.All(id => id != saleDto.CarId.Value)) //check all dbCarIds against carDto id
            {
                continue;
            }

            Sale sale = mapper.Map<Sale>(saleDto);

            sales.Add(sale);
        }

        context.Sales.AddRange(sales);

        context.SaveChanges();

        return $"Successfully imported {sales.Count}";
    }

    public static string GetCarsWithDistance(CarDealerContext context)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context
            .Cars
            .Where(c => c.TraveledDistance > 2_000_000)
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .ProjectTo<ExportCarDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetCarsFromMakeBmw(CarDealerContext context)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context
            .Cars
            .Where(c => c.Make == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .ProjectTo<ExportCarByBMWBrand>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetLocalSuppliers(CarDealerContext context)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();
        var localSuppliers = context
            .Suppliers
            .Where(s => s.IsImporter == false)
            .ProjectTo<ExportLocalSupplier>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(localSuppliers, "suppliers");
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext context)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var cars = context
            .Cars
            .Include(c => c.PartsCars)
            .OrderByDescending(c => c.TraveledDistance)
            .ThenBy(c => c.Model)
            .Take(5)
            .ProjectTo<ExportCarWithPartsDto>(mapper.ConfigurationProvider)
            .ToArray();

        return xmlHelper.Serialize(cars, "cars");
    }

    public static string GetTotalSalesByCustomer(CarDealerContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var customersRaw = context
            .Customers
            .Where(s => s.Sales.Any())
            .Select(s => new
            {
                FullName = s.Name,
                BoughtCars = s.Sales.Count,
                SpentMoney = s.Sales.SelectMany(c => c.Car.PartsCars).Sum(p => p.Part.Price),
                IsYoung = s.IsYoungDriver,
            })
            .ToArray();

        ICollection<ExportCustomerTotalSaleDto> customers = new List<ExportCustomerTotalSaleDto>();

        foreach (var c in customersRaw)
        {
            var customer = new ExportCustomerTotalSaleDto()
            {
                FullName = c.FullName,
                BoughtCars = c.BoughtCars,
                SpentMoney = c.IsYoung
                    ? Math.Round(c.SpentMoney * (decimal)0.95, 2, MidpointRounding.ToZero)
                    : c.SpentMoney,
            };

            customers.Add(customer);
        }

        var customerOutput = customers.OrderByDescending(c => c.SpentMoney).ToArray();

        return xmlHelper.Serialize(customerOutput, "customers");
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext context)
    {
        IMapper mapper = InitializeMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var sales = context
            .Sales
            .Include(s => s.Customer)
            .Select(s => new ExportSalesWithDiscountDto()
            {
                SaleCar = new ExportSaleCarDto()
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TraveledDistance = s.Car.TraveledDistance,
                },
                Discount = s.Discount,
                CustomerName = s.Customer.Name,
                Price = s.Car.PartsCars.Sum(s => s.Part.Price),
                PriceWithDiscount = Math.Round((double)(s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100)), 4),
            })
            .ToArray();

        return xmlHelper.Serialize(sales, "sales");
    }
    private static IMapper InitializeMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
}
