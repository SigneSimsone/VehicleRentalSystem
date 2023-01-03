using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data.Requests
{
    public class CarSearchRequest
    {
        public BrandModel? Brand { get; set; }
        public GearboxModel? GearboxType { get; set; }
        public FuelTypeModel? FuelType { get; set; }
        public LocationModel? Location { get; set; }

        public int? Year { get; set; }
        public float? FuelConsumption { get; set; }
        public int? Mileage { get; set; }
        public int? Passengers { get; set; }
        public decimal? DailyPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
