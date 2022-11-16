using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Data.Managers
{
    public class CarDataManager
    {
        private readonly ApplicationDbContext _dbContext;

        public CarDataManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

      /*public CarModel[] GetCars()
        {
            var result = _dbContext
                .Artists
                .Include(x => x.Users)
                .ToArray();

            return result;
        }*/

        internal void AddCar(BrandModel brand, CarModelModel model, GearboxModel gearbox, FuelTypeModel fuelType, int year, string registrationNumber, int fuelConsumption, int mileage, int passengers, int luggage, int doors, bool airConditioner, bool availability, float dailyPrice, string imagePath, LocationModel location)
        {
            var item = new CarModel()
            {
                Brand = brand,
                Model = model,
                GearboxType = gearbox,
                FuelType = fuelType,
                Year = year,
                RegistrationNumber = registrationNumber,
                FuelConsumption = fuelConsumption,
                Mileage = mileage,
                Passengers = passengers,
                Luggage = luggage,
                Doors = doors,
                AirConditioner = airConditioner,
                Availability = availability,
                DailyPrice = dailyPrice,
                Location = location,
                ImagePath = imagePath
            };

            _dbContext.Cars.Add(item);
            _dbContext.SaveChanges();
        }


        internal void AddBrand(string brand)
        {
            var item = new BrandModel()
            {
                Brand = brand
            };

            _dbContext.Brands.Add(item);
            _dbContext.SaveChanges();
        }
        public BrandModel GetOneBrand(Guid Id)
        {
            var item = _dbContext.Brands.First(x => x.Id == Id);

            return item;
        }


        internal void AddFuelType(string fuelType)
        {
            var item = new FuelTypeModel()
            {
                FuelType = fuelType
            };

            _dbContext.FuelTypes.Add(item);
            _dbContext.SaveChanges();
        }
        public FuelTypeModel GetOneFuelType(Guid Id)
        {
            var item = _dbContext.FuelTypes.First(x => x.Id == Id);

            return item;
        }


        internal void AddGearboxType(string gearbox)
        {
            var item = new GearboxModel()
            {
                Gearbox = gearbox
            };

            _dbContext.GearboxTypes.Add(item);
            _dbContext.SaveChanges();
        }
        public GearboxModel GetOneGearboxType(Guid Id)
        {
            var item = _dbContext.GearboxTypes.First(x => x.Id == Id);

            return item;
        }


        internal void AddCarModel(string model)
        {
            var item = new CarModelModel()
            {
                Model = model
            };

            _dbContext.CarModels.Add(item);
            _dbContext.SaveChanges();
        }
        public CarModelModel GetOneCarModel(Guid Id)
        {
            var item = _dbContext.CarModels.First(x => x.Id == Id);

            return item;
        }

        internal void AddLocation(string city, string street, string number)
        {
            var item = new LocationModel()
            {
                City = city,
                Street = street,
                Number = number
            };

            _dbContext.Locations.Add(item);
            _dbContext.SaveChanges();
        }
        public LocationModel GetOneLocation(Guid Id)
        {
            var item = _dbContext.Locations.First(x => x.Id == Id);

            return item;
        }
    }
}
