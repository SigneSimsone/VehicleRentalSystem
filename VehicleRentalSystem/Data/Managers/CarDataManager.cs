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
            var item = _dbContext.Brands.FirstOrDefault(x => x.Id == Id);

            return item;
        }
        internal void EditBrand(Guid id, string brand)
        {
            var item = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
            item.Brand = brand;

            _dbContext.SaveChanges();
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
        internal void EditFuelType(Guid id, string fuelType)
        {
            var item = _dbContext.FuelTypes.FirstOrDefault(x => x.Id == id);
            item.FuelType = fuelType;

            _dbContext.SaveChanges();
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
        internal void EditGearboxType(Guid id, string gearbox)
        {
            var item = _dbContext.GearboxTypes.FirstOrDefault(x => x.Id == id);
            item.Gearbox = gearbox;

            _dbContext.SaveChanges();
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
        internal void EditCarModel(Guid id, string model)
        {
            var item = _dbContext.CarModels.FirstOrDefault(x => x.Id == id);
            item.Model = model;

            _dbContext.SaveChanges();
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
        internal void EditLocation(Guid id, string city, string street, string number)
        {
            var item = _dbContext.Locations.FirstOrDefault(x => x.Id == id);
            item.City = city;
            item.Street = street;
            item.Number = number;

            _dbContext.SaveChanges();
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
            IQueryable<CarModel>? cars = _dbContext
                .Cars.Include(x => x.Brand)
                     .Include(x => x.FuelType)
                     .Include(x => x.GearboxType)
                     .Include(x => x.Model)
                     .Include(x => x.Location)
                     .Include(x => x.Reservations)
                     .AsQueryable();

            if (model.Brand != null)
            {
                cars = cars.Where(x => x.Brand.Brand.Contains(model.Brand.Brand));
            }

            if (model.GearboxType != null)
            {
                cars = cars.Where(x => x.GearboxType.Gearbox.Contains(model.GearboxType.Gearbox));
            }

            if (model.FuelType != null)
            {
                cars = cars.Where(x => x.FuelType.FuelType.Contains(model.FuelType.FuelType));
            }

            if (model.Location != null)
            {
                cars = cars.Where(x => x.Location.City.Contains(model.Location.City));
            }

            if (model.Location != null)
            {
                cars = cars.Where(x => x.Location.Street.Contains(model.Location.Street));
            }

            if (model.Location != null)
            {
                cars = cars.Where(x => x.Location.Number.Contains(model.Location.Number));
            }

            if (model.Year.HasValue)
            {
                cars = cars.Where(x => x.Year == model.Year.Value);
            }

            if (model.FuelConsumption.HasValue)
            {
                cars = cars.Where(x => x.FuelConsumption <= model.FuelConsumption.Value);
            }

            if (model.Mileage.HasValue)
            {
                cars = cars.Where(x => x.Mileage <= model.Mileage.Value);
            }

            if (model.Passengers.HasValue)
            {
                cars = cars.Where(x => x.Passengers >= model.Passengers.Value);
            }

            if (model.DailyPrice.HasValue)
            {
                cars = cars.Where(x => x.DailyPrice <= model.DailyPrice.Value);
            }

            CarModel[]? carList = cars.Where(x => x.Availability == true).ToArray();
            List<CarModel> result = new List<CarModel>();

            if (model.StartDate.HasValue || model.EndDate.HasValue)
            {
                var carsWithoutReservations = carList.Where(t => !t.Reservations.Any()).ToList();
                result.AddRange(carsWithoutReservations);
            }
            else
            {
                //if reservation dates are not requested - return list of cars that meet requested requirements
                return carList.ToArray();
            }

            if (model.StartDate.HasValue)
            {
                var carsWithReservation = carList.Where(t => t.Reservations.Any() && t.Reservations.All(x =>
                (x.StartDate > model.StartDate.Value && x.EndDate > model.StartDate.Value) ||
                (x.StartDate < model.StartDate.Value && x.EndDate < model.StartDate.Value)));

                result.AddRange(carsWithReservation);
            }

            if (model.EndDate.HasValue)
            {
                var carsWithReservation = result.Where(t => t.Reservations.Any() && t.Reservations.All(x =>
                (x.StartDate > model.EndDate.Value && x.EndDate > model.EndDate.Value) ||
                (x.StartDate < model.EndDate.Value && x.EndDate < model.EndDate.Value)));

                result.AddRange(carsWithReservation);
            }

            return result.ToArray();
        }


        internal CarModel[] CheckIfDatesValid(Guid carId, DateTime requestedStartDate, DateTime requestedEndDate)
        {
            IQueryable<CarModel>? cars = _dbContext
                .Cars.Include(x => x.Brand)
                     .Include(x => x.FuelType)
                     .Include(x => x.GearboxType)
                     .Include(x => x.Model)
                     .Include(x => x.Location)
                     .Include(x => x.Reservations)
                     .AsQueryable();

            CarModel[]? carList = cars.Where(x => x.Id == carId).ToArray();
            List<CarModel> result = new List<CarModel>();

            var carsWithReservation = carList.Where(t => t.Reservations.Any() && t.Reservations.All(x =>
            ((x.StartDate > requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate > requestedEndDate)) ||
            ((x.StartDate < requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate > requestedEndDate)) ||
            ((x.StartDate < requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate < requestedEndDate)) ||
            ((x.StartDate > requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate < requestedEndDate))));

            result.AddRange(carsWithReservation);

            return result.ToArray();
        }


        public ReservationModel[] GetReservations()
        {
            var result = _dbContext
                .Reservations
                .Include(x => x.Payment)
                .Include(x => x.Car)
                .ToArray();

            return result;
        }

        public ReservationModel[] GetReservations(Guid carId)
        {
            var result = _dbContext
                .Reservations
                .Include(x => x.Car)
                .Include(x => x.Payment)
                .Include(x => x.User)
                .Where(x => x.Car.Id == carId)
                .ToArray();

            return result;
        }

        public ReservationModel GetOneReservation(Guid reservationId)
        {
            var item = _dbContext
                .Reservations
                .Include(x => x.Car)
                .Include(x => x.Payment)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == reservationId);

            return item;
        }

        internal Guid AddReservation(DateTime startDate, DateTime endDate, Guid carId, UserModel user, PaymentModel payment)
        {
            var car = GetOneCar(carId);
            var item = new ReservationModel()
            {
                StartDate = startDate,
                EndDate = endDate,
                User = user,
                Car = car,
                Payment = payment
            };

            _dbContext.Reservations.Add(item);
            _dbContext.SaveChanges();
            return item.Id;
        }

        internal Guid AddPayment(double amount, DateTime startDate, DateTime endDate, Guid carId)
        {
            //var reservation = GetOneReservation(startDate, endDate, carId);
            var item = new PaymentModel()
            {
                Date = DateTime.Now,
                Amount = amount
            };

            _dbContext.Payments.Add(item);
            _dbContext.SaveChanges();
            return item.Id;
        }

        /*        public PaymentModel GetPayment(double amount, DateTime startDate, DateTime endDate, Guid carId)
                {
                    var item = _dbContext
                        .Payments
                        .Include(x => x.Reservation)
                        .Where(x => x.Amount == amount)
                        .Where(x => x.Reservation.StartDate == startDate)
                        .Where(x => x.Reservation.EndDate == endDate)
                        .Where(x => x.Reservation.Car.Id == carId);

                    return item;
                }*/

        public PaymentModel GetPayment(Guid paymentId)
        {
            var item = _dbContext
                .Payments
                .FirstOrDefault(x => x.Id == paymentId);

            return item;
        }
    }
}