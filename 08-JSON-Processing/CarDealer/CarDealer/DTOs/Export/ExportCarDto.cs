using CarDealer.Models;

namespace CarDealer.DTOs.Export
{
    public class ExportCarDto
    {
        public ExportCarDto(Car car)
        {
            this.Id = car.Id;
            this.Make = car.Make;
            this.Model = car.Model;
            this.TraveledDistance = car.TraveledDistance;
        }
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public long TraveledDistance { get; set; }
    }
}
