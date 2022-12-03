using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Data.Requests;
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
                .Include(x => x.Brand)
                .Include(x => x.FuelType)
                .Include(x => x.GearboxType)
                .Include(x => x.Model)
                .Include(x => x.Location)
                .ToArray();

            return result;
        }

        public CarModel[] GetCars(List<Guid> carsId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.Brand)
                .Include(x => x.FuelType)
                .Include(x => x.GearboxType)
                .Include(x => x.Model)
                .Include(x => x.Location)
                .Where(x => carsId.Contains(x.Id))
                .ToArray();

            return result;
        }

        public CarModel GetOneCar(Guid id)
        {
            var item = _dbContext
                .Cars
                .Include(x => x.Brand)
                .Include(x => x.FuelType)
                .Include(x => x.GearboxType)
                .Include(x => x.Model)
                .Include(x => x.Location)
                .Include(x => x.Feedbacks)
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
            item.Location = location;

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


        internal CarModel[] SearchCars(CarSearchRequest model)
        {
            var cars = _dbContext
                .Cars.Include(x => x.Brand)
                     .Include(x => x.FuelType)
                     .Include(x => x.GearboxType)
                     .Include(x => x.Model)
                     .Include(x => x.Location)
                     .AsQueryable();

            if (model.Brand != null && model.Brand.Brand != "" && model.Brand.Brand != null)
            {
                cars = cars.Where(x => x.Brand.Brand.Contains(model.Brand.Brand));
            }

            if (model.GearboxType != null && model.GearboxType.Gearbox != "" && model.GearboxType.Gearbox != null)
            {
                cars = cars.Where(x => x.GearboxType.Gearbox.Contains(model.GearboxType.Gearbox));
            }

            if (model.FuelType != null && model.FuelType.FuelType != "" && model.FuelType.FuelType != null)
            {
                cars = cars.Where(x => x.FuelType.FuelType.Contains(model.FuelType.FuelType));
            }

            if (model.Location != null && model.Location.City != "" && model.Location.City != null)
            {
                cars = cars.Where(x => x.Location.City.Contains(model.Location.City));
            }

            if (model.Location != null && model.Location.Street != "" && model.Location.Street != null)
            {
                cars = cars.Where(x => x.Location.Street.Contains(model.Location.Street));
            }

            if (model.Location != null && model.Location.Number != "" && model.Location.Number != null)
            {
                cars = cars.Where(x => x.Location.Number.Contains(model.Location.Number));
            }

            if (model.Year != 0)
            {
                cars = cars.Where(x => x.Year == model.Year);
            }

            if (model.FuelConsumption != 0)
            {
                cars = cars.Where(x => x.FuelConsumption <= model.FuelConsumption);
            }

            if (model.Mileage != 0)
            {
                cars = cars.Where(x => x.Mileage <= model.Mileage);
            }

            if (model.Passengers != 0)
            {
                cars = cars.Where(x => x.Passengers >= model.Passengers);
            }

            if (model.DailyPrice != 0)
            {
                cars = cars.Where(x => x.DailyPrice <= model.DailyPrice);
            }

            cars = cars.Where(x => x.Availability == true);

            return cars.ToArray();
        }
    }
}
