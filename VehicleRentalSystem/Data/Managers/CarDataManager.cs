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

        public CarModel[] GetCarsByRegNr(string registrationNumber)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.Brand)
                .Include(x => x.FuelType)
                .Include(x => x.GearboxType)
                .Include(x => x.Model)
                .Include(x => x.Location)
                .Include(x => x.Feedbacks)
                .Where(x => x.RegistrationNumber == registrationNumber)
                .ToArray();

            return result;
        }

        public CarModel[] GetCarsByBrand(Guid brandId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.Brand)
                .Where(x => x.Brand.Id == brandId)
                .ToArray();

            return result;
        }
        public CarModel[] GetCarsByCarModel(Guid carModelId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.Model)
                .Where(x => x.Model.Id == carModelId)
                .ToArray();

            return result;
        }
        public CarModel[] GetCarsByFuelType(Guid fuelTypeId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.FuelType)
                .Where(x => x.FuelType.Id == fuelTypeId)
                .ToArray();

            return result;
        }
        public CarModel[] GetCarsByGearboxType(Guid gearboxTypeId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.GearboxType)
                .Where(x => x.GearboxType.Id == gearboxTypeId)
                .ToArray();

            return result;
        }
        public CarModel[] GetCarsByLocation(Guid locationId)
        {
            var result = _dbContext
                .Cars
                .Include(x => x.Location)
                .Where(x => x.Location.Id == locationId)
                .ToArray();

            return result;
        }


        internal void AddCar(BrandModel brand,
                             CarModelModel model,
                             GearboxModel gearbox,
                             FuelTypeModel fuelType,
                             int year,
                             string registrationNumber,
                             float fuelConsumption,
                             int mileage,
                             int passengers,
                             int luggage,
                             int doors,
                             bool airConditioner,
                             bool availability,
                             decimal dailyPrice,
                             string imagePath,
                             LocationModel location)
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

        //edit car info
        internal void Edit(Guid id,
                           BrandModel brand,
                           CarModelModel model,
                           GearboxModel gearbox,
                           FuelTypeModel fuelType,
                           int year,
                           string registrationNumber,
                           float fuelConsumption,
                           int mileage,
                           int passengers,
                           int luggage,
                           int doors,
                           bool airConditioner,
                           bool availability,
                           decimal dailyPrice,
                           string imagePath,
                           LocationModel location)
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
            if (!string.IsNullOrEmpty(imagePath))
            {
                item.ImagePath = imagePath;
            }
            item.Location = location;

            _dbContext.SaveChanges();
        }

        //delete car
        internal void Delete(Guid id)
        {
            var item = _dbContext
                .Cars
                .Include(x => x.Feedbacks)
                .FirstOrDefault(x => x.Id == id);

            item.Feedbacks.Clear();
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
        internal void DeleteBrand(Guid brandId)
        {
            var item = _dbContext
                .Brands
                .FirstOrDefault(x => x.Id == brandId);

            _dbContext.Brands.Remove(item);
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
        internal void DeleteFuelType(Guid fuelTypeId)
        {
            var item = _dbContext
                .FuelTypes
                .FirstOrDefault(x => x.Id == fuelTypeId);

            _dbContext.FuelTypes.Remove(item);
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
        internal void DeleteGearboxType(Guid gearboxTypeId)
        {
            var item = _dbContext
                .GearboxTypes
                .FirstOrDefault(x => x.Id == gearboxTypeId);

            _dbContext.GearboxTypes.Remove(item);
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
        internal void DeleteCarModel(Guid carModelId)
        {
            var item = _dbContext
                .CarModels
                .FirstOrDefault(x => x.Id == carModelId);

            _dbContext.CarModels.Remove(item);
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
        internal void DeleteLocation(Guid locationId)
        {
            var item = _dbContext
                .Locations
                .FirstOrDefault(x => x.Id == locationId);

            _dbContext.Locations.Remove(item);
            _dbContext.SaveChanges();
        }



        internal void AddFeedback(Guid Id, string comment, UserModel user)
        {
            var car = GetOneCar(Id);
            var item = new FeedbackModel()
            {
                Comment = comment,
                Date = DateTime.UtcNow,
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

            //check for the requirements and get all the cars that match
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

            //if only start date has input
            if (model.StartDate.HasValue && model.EndDate == null)
            {               
                var carsWithReservation = carList.Where(t => t.Reservations.Any() &&
                                                            t.Reservations.All(x =>
                                                                (x.StartDate < model.StartDate.Value && x.EndDate < model.StartDate.Value)));

                result.AddRange(carsWithReservation);
            }

            //if only end date has input
            if (model.EndDate.HasValue && model.StartDate == null)
            {
                var carsWithReservation = carList.Where(t => t.Reservations.Any() && 
                                                            t.Reservations.All(x =>
                                                                (x.EndDate > model.EndDate.Value && x.StartDate > model.EndDate.Value) ||
                                                                (x.EndDate < model.EndDate.Value && x.StartDate < model.EndDate.Value && x.EndDate < DateTime.UtcNow)));

                result.AddRange(carsWithReservation);
            }

            //if start date and end date has input
            if (model.StartDate.HasValue && model.EndDate.HasValue)
            {               
                var carsWithReservation = carList.Where(t => t.Reservations.Any() && t.Reservations.All(x =>
               (x.StartDate < model.StartDate.Value && x.EndDate < model.StartDate.Value) ||
               (x.EndDate > model.EndDate.Value && x.StartDate > model.EndDate.Value)));

                result.AddRange(carsWithReservation);
            }

            return result.ToArray();
        }


        internal ReservationModel[] CheckIfDatesValid(Guid carId, DateTime requestedStartDate, DateTime requestedEndDate)
        {//check if car has reservations in the selected period
            IQueryable<ReservationModel>? reservations = _dbContext
                     .Reservations
                     .Include(x => x.Car)
                     .Include(x => x.User)
                     .Include(x => x.Payment)
                     .AsQueryable();

            ReservationModel[]? reservationList = reservations.Where(x => x.Car.Id == carId).ToArray();
            List<ReservationModel> result = new List<ReservationModel>();

            var ReservationInPeriod = reservationList.Where(x =>
            ((x.StartDate > requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate > requestedEndDate)) ||
            ((x.StartDate < requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate > requestedEndDate)) ||
            ((x.StartDate < requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate < requestedEndDate)) ||
            ((x.StartDate > requestedStartDate && x.EndDate > requestedStartDate) && (x.StartDate < requestedEndDate && x.EndDate < requestedEndDate)));

            result.AddRange(ReservationInPeriod);

            return result.ToArray();
        }


        public ReservationModel[] GetReservations()
        {
            var result = _dbContext
                .Reservations
                .Include(x => x.Payment)
                .Include(x => x.User)
                .Include(x => x.Car)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Car)
                .ThenInclude(x => x.Model)
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
                .Where(x => x.EndDate >= DateTime.UtcNow)
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

        internal ReservationModel[] GetUserReservations(UserModel user)
        {
            var result = _dbContext
                .Reservations
                .Include(x => x.Car)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Car)
                .ThenInclude(x => x.Model)
                .Include(x => x.User)
                .Where(x => x.User == user)
                .ToArray();

            return result;
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

        internal void DeleteReservation(Guid reservationId)
        {
            var item = _dbContext
                .Reservations
                .FirstOrDefault(x => x.Id == reservationId);

            _dbContext.Reservations.Remove(item);
            _dbContext.SaveChanges();
        }

        internal void DeleteUserReservations(UserModel user)
        {
            var items = _dbContext
                .Reservations
                .Where(x => x.User == user)
                .ToArray();

            _dbContext.Reservations.RemoveRange(items);
            _dbContext.SaveChanges();
        }


        internal Guid AddPayment(decimal amount)
        {
            var item = new PaymentModel()
            {
                Date = null,
                Amount = amount
            };

            _dbContext.Payments.Add(item);
            _dbContext.SaveChanges();
            return item.Id;
        }

        public PaymentModel GetPayment(Guid paymentId)
        {
            var item = _dbContext
                .Payments
                .FirstOrDefault(x => x.Id == paymentId);

            return item;
        }

        internal void AddPaymentDate(Guid paymentId, DateTime paymentDate)
        {
            var item = _dbContext.Payments.FirstOrDefault(x => x.Id == paymentId);
            item.Date = paymentDate;

            _dbContext.SaveChanges();
        }
    }
}