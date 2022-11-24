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

        public CarModel[] GetCars()
        {
            var result = _dbContext
                .Cars
                .ToArray();

            return result;
        }

        public CarModel GetOneCar(Guid id)
        {
            var item = _dbContext.Cars.Include(x => x.Brand)
                                          .Include(x => x.FuelType)
                                          .Include(x => x.GearboxType)
                                          .Include(x => x.Model)
                                          .Include(x => x.Location)
                                          .First(x => x.Id == id);

            return item;
        }

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

        internal void Edit(Guid id, BrandModel brand, CarModelModel model, GearboxModel gearbox, FuelTypeModel fuelType, int year, string registrationNumber, int fuelConsumption, int mileage, int passengers, int luggage, int doors, bool airConditioner, bool availability, float dailyPrice, string imagePath, LocationModel location)
        {
            var item = _dbContext.Cars.First(x => x.Id == id);
            item.Brand = brand;
            item.Model = model;
            item.GearboxType = gearbox;
            item.FuelType = fuelType;
            item.Year = year;
            item.RegistrationNumber = registrationNumber;
            item.FuelConsumption = fuelConsumption;
            item.Mileage = mileage;
            item.Passengers = passengers;
            item.Luggage = luggage;
            item.Doors = doors;
            item.AirConditioner = airConditioner;
            item.Availability = availability;
            item.DailyPrice = dailyPrice;
            item.ImagePath = imagePath;
            item.Location = location ;

            _dbContext.SaveChanges();
        }

        internal void Delete(Guid id)
        {
            var item = _dbContext
                .Cars
                .Include(x => x.Feedbacks)
                .Include(x => x.Reservations)
                .First(x => x.Id == id);

            item.Feedbacks.Clear();
            _dbContext.SaveChanges();

            item.Reservations.Clear();
            _dbContext.SaveChanges();

            _dbContext.Cars.Remove(item);
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
        public BrandModel[] GetBrands()
        {
            var result = _dbContext
                .Brands
                .ToArray();

            return result;
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
        public FuelTypeModel[] GetFuelTypes()
        {
            var result = _dbContext
                .FuelTypes
                .ToArray();

            return result;
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
        public GearboxModel[] GetGearboxTypes()
        {
            var result = _dbContext
                .GearboxTypes
                .ToArray();

            return result;
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
        public CarModelModel[] GetCarModels()
        {
            var result = _dbContext
                .CarModels
                .ToArray();

            return result;
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
        public LocationModel[] GetLocations()
        {
            var result = _dbContext
                .Locations
                .ToArray();

            return result;
        }
        public LocationModel GetOneLocation(Guid Id)
        {
            var item = _dbContext.Locations.First(x => x.Id == Id);

            return item;
        }

        internal void AddFeedback(Guid Id, string comment, UserModel user)
        {
            var car = GetOneCar(Id);
            var item = new FeedbackModel()
            {
                Comment = comment,
                Date = DateTime.Now,
                User = user,
                Car = car
            };

            _dbContext.Feedbacks.Add(item);
            _dbContext.SaveChanges();
        }

        internal void DeleteFeedback(Guid Id)
        {
            var item = _dbContext.Feedbacks.First(x => x.Id == Id);
            _dbContext.Feedbacks.Remove(item);

            _dbContext.SaveChanges();
        }
    }
}
