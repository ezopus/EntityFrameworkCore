using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Suppliers
            this.CreateMap<ImportSupplierDto, Supplier>();
            this.CreateMap<Supplier, ExportLocalSupplier>()
                .ForMember(d => d.PartsCount,
                    opt => opt.MapFrom(s => s.Parts.Count));

            //Parts
            this.CreateMap<ImportPartDto, Part>();
            this.CreateMap<Part, ExportCarPartListDto>()
                .ForMember(d => d.Price, opt =>
                    opt.MapFrom(s => Math.Round(s.Price, 2)));

            //Car
            //let automapper ignore validation for specific xml element with children
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            this.CreateMap<Car, ExportCarDto>();
            this.CreateMap<Car, ExportCarByBMWBrand>();

            this.CreateMap<Car, ExportCarPartListDto>();
            this.CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(d => d.Parts, opt =>
                    opt.MapFrom(s => s.PartsCars
                        .Select(pc => pc.Part)
                        .OrderByDescending(p => p.Price)
                        .ToArray()));

            this.CreateMap<Car, ExportSaleCarDto>();

            //Customers
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate,
                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            this.CreateMap<Customer, ExportCustomerTotalSaleDto>();

            //Sales
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId,
                    opt => opt.MapFrom(s => s.CarId.Value));

            this.CreateMap<Sale, ExportSalesWithDiscountDto>()
                .ForMember(d => d.SaleCar,
                    opt => opt.MapFrom(s => s.Car))
                .ForMember(d => d.Discount,
                    opt => opt.MapFrom(s => s.Discount))
                .ForMember(dest => dest.CustomerName,
                    opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(c =>
                        Math.Round(c.Car.PartsCars
                        .Select(pc => pc.Part.Price)
                        .Sum(), 2)))
                .ForMember(dest => dest.PriceWithDiscount,
                    opt => opt.MapFrom(c =>
                        Math.Round(c.Car.PartsCars
                        .Select(cp => cp.Part.Price)
                        .Sum() * (1 - c.Discount / 100), 2, MidpointRounding.AwayFromZero)));
        }
    }
}
