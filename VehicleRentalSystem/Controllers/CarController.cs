using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class CarController : Controller
    {
        private readonly CarDataManager _carDataManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IWebHostEnvironment _hostingEnv;

        public CarController(CarDataManager carDataManager, UserManager<UserModel> userManager, IWebHostEnvironment hostingEnv)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
            _hostingEnv = hostingEnv;
        }

        [HttpGet]
        public IActionResult Index(CarViewModel viewModel = null)
        {
            CarModel[] cars = _carDataManager.GetCars();
            var userId = _userManager.GetUserId(User);

            if(viewModel == null)
            {
                viewModel = new CarViewModel();
            }
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddCar(CarViewModel model)
        {
             if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            CarModel[] cars = _carDataManager.GetCarsByRegNr(model.RegistrationNumber);

            if (cars.Any())
            {
                model.SameRegNrMessage = "Car with the same registration number already exists!";
                return View("AddCar", model);
            }

            string relativeFilePath = null;
            BrandModel brandmodel = _carDataManager.GetOneBrand(model.SelectedBrand);
            CarModelModel carmodelmodel = _carDataManager.GetOneCarModel(model.SelectedCarModel);
            GearboxModel gearboxmodel = _carDataManager.GetOneGearboxType(model.SelectedGearboxType);
            FuelTypeModel fueltypemodel = _carDataManager.GetOneFuelType(model.SelectedFuelType);
            LocationModel locationmodel = _carDataManager.GetOneLocation(model.SelectedLocation);

            if (model.File != null)
            {
                //upload files to wwwroot
                string fileName = $"{brandmodel.Brand}_{carmodelmodel.Model}.jpg".Replace(" ", "");
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "img", fileName);

                using (var fileSteam = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileSteam);
                }

                relativeFilePath = Path.Combine("\\images", fileName);
            }

            _carDataManager.AddCar(brandmodel, carmodelmodel, gearboxmodel, fueltypemodel, model.Year.Value, model.RegistrationNumber, model.FuelConsumption.Value, model.Mileage.Value, model.Passengers.Value, model.Luggage.Value, model.Doors.Value, model.AirConditioner, model.Availability, model.DailyPrice.Value, relativeFilePath, locationmodel);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid CarId, CarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index), model);
            }

            string relativeFilePath = null;
            BrandModel brandmodel = _carDataManager.GetOneBrand(model.SelectedBrand);
            CarModelModel carmodelmodel = _carDataManager.GetOneCarModel(model.SelectedCarModel);
            GearboxModel gearboxmodel = _carDataManager.GetOneGearboxType(model.SelectedGearboxType);
            FuelTypeModel fueltypemodel = _carDataManager.GetOneFuelType(model.SelectedFuelType);
            LocationModel locationmodel = _carDataManager.GetOneLocation(model.SelectedLocation);

            if (model.File != null)
            {
                //upload files to wwwroot
                string fileName = $"{brandmodel.Brand}_{carmodelmodel.Model}.jpg".Replace(" ", "");
                var absoluteFilePath = Path.Combine(_hostingEnv.WebRootPath, "images", fileName);

                using (var fileSteam = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileSteam);
                }

                relativeFilePath = Path.Combine("\\images", fileName);
            }

            _carDataManager.Edit(CarId, brandmodel, carmodelmodel, gearboxmodel, fueltypemodel, model.Year.Value, model.RegistrationNumber, model.FuelConsumption.Value, model.Mileage.Value, model.Passengers.Value, model.Luggage.Value, model.Doors.Value, model.AirConditioner, model.Availability, model.DailyPrice.Value, relativeFilePath, locationmodel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(Guid CarId)
        {
            // get car from database (CarModel)
            CarModel car = _carDataManager.GetOneCar(CarId);
            CarViewModel viewModel = new CarViewModel();
            viewModel.CarId = car.Id;

            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand", car.Brand.Brand);

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model", car.Model.Model);

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox", car.GearboxType.Gearbox);

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType", car.FuelType.FuelType);

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation", car.Location.FullLocation);

            viewModel.Year = car.Year;
            viewModel.RegistrationNumber = car.RegistrationNumber;
            viewModel.FuelConsumption = car.FuelConsumption;
            viewModel.Mileage = car.Mileage;
            viewModel.Passengers = car.Passengers;
            viewModel.Luggage = car.Luggage;
            viewModel.Doors = car.Doors;
            viewModel.AirConditioner = car.AirConditioner;
            viewModel.Availability = car.Availability;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.ImagePath = car.ImagePath;

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult OpenCar(Guid CarId)
        {
            if (CarId == Guid.Empty)
            {
                return RedirectToAction(nameof(Index));
            }

            // get car from database (CarModel)
            CarModel model = _carDataManager.GetOneCar(CarId);

            return View("OneCar", model);
        }

        [HttpPost]
        public IActionResult Delete(Guid CarId)
        {
            ReservationModel[] reservations = _carDataManager.GetReservations(CarId);

            if(reservations.Any())
            {
                //CarViewModel viewModel = new CarViewModel();
                //viewModel.ActiveReservationsMessage = "Can't delete a car with active reservations!";
                //return RedirectToAction(nameof(Index), viewModel);
                ModelState.AddModelError("ActiveReservationsMessage", "Can't delete a car with active reservations!");
                return RedirectToAction(nameof(Index));
            }

            _carDataManager.Delete(CarId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ShowFilteredCars(Guid[] carsId)
        {
            var userId = _userManager.GetUserId(User);
            CarModel[] cars = _carDataManager.GetCars(carsId.ToList());

            CarViewModel viewModel = new CarViewModel();
            viewModel.Cars = cars;
            viewModel.UserId = userId;

            if(!cars.Any())
            {
                viewModel.NoCarFoundMessage = "No cars were found!";
            }

            return View("Index", viewModel);
        }


        [HttpGet]
        public IActionResult OpenAddCar(CarViewModel viewModel = null)
        {
            BrandModel[] brands = _carDataManager.GetBrands();
            var brandList = brands.Select(x => new { x.Id, x.Brand }).ToList();
            viewModel.BrandDropdown = new SelectList(brandList, "Id", "Brand");

            CarModelModel[] carModels = _carDataManager.GetCarModels();
            var carModelList = carModels.Select(x => new { x.Id, x.Model }).ToList();
            viewModel.CarModelDropdown = new SelectList(carModelList, "Id", "Model");

            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            var gearboxTypeList = gearboxTypes.Select(x => new { x.Id, x.Gearbox }).ToList();
            viewModel.GearboxTypeDropdown = new SelectList(gearboxTypeList, "Id", "Gearbox");

            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            var fuelTypeList = fuelTypes.Select(x => new { x.Id, x.FuelType }).ToList();
            viewModel.FuelTypeDropdown = new SelectList(fuelTypeList, "Id", "FuelType");

            LocationModel[] locations = _carDataManager.GetLocations();
            var locationList = locations.Select(x => new { x.Id, x.FullLocation }).ToList();
            viewModel.LocationDropdown = new SelectList(locationList, "Id", "FullLocation");

            viewModel.FuelConsumption = null;

            return View("AddCar", viewModel);
        }

        [HttpGet]
        public IActionResult OpenAddCarProperties()
        {
            return View("AddCarProperties");
        }

        [HttpPost]
        public IActionResult AddBrand(string Brand)
        {
            _carDataManager.AddBrand(Brand);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddFuelType(string FuelType)
        {
            _carDataManager.AddFuelType(FuelType);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddGearboxType(string Gearbox)
        {
            _carDataManager.AddGearboxType(Gearbox);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddCarModel(string CarModel)
        {
            _carDataManager.AddCarModel(CarModel);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult AddLocation(string City, string Street, string Number)
        {
            _carDataManager.AddLocation(City, Street, Number);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult ShowCarProperties(CarPropertiesViewModel viewModel = null)
        {
            BrandModel[] brands = _carDataManager.GetBrands();
            CarModelModel[] models = _carDataManager.GetCarModels();
            FuelTypeModel[] fuelTypes = _carDataManager.GetFuelTypes();
            GearboxModel[] gearboxTypes = _carDataManager.GetGearboxTypes();
            LocationModel[] locations = _carDataManager.GetLocations();


            if (viewModel == null)
            {
                viewModel = new CarPropertiesViewModel();
            }
            viewModel.Brands = brands;
            viewModel.Models = models;
            viewModel.FuelTypes = fuelTypes;
            viewModel.GearboxTypes = gearboxTypes;
            viewModel.Locations = locations;

            return View("ShowCarProperties", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditBrand(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}

            _carDataManager.EditBrand(model.BrandId, model.Brand);
            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditBrand(Guid BrandId)
        {
            // get brand from database
            BrandModel brand = _carDataManager.GetOneBrand(BrandId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.BrandId = brand.Id;
            viewModel.Brand = brand.Brand;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}

            _carDataManager.EditCarModel(model.CarModelId, model.CarModel);
            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditModel(Guid CarModelId)
        {
            // get car model from database
            CarModelModel carModel = _carDataManager.GetOneCarModel(CarModelId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.CarModelId = carModel.Id;
            viewModel.CarModel = carModel.Model;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditFuelType(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}

            _carDataManager.EditFuelType(model.FuelTypeId, model.FuelType);
            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditFuelType(Guid FuelTypeId)
        {
            // get fuel type from database
            FuelTypeModel fuelType = _carDataManager.GetOneFuelType(FuelTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.FuelTypeId = fuelType.Id;
            viewModel.FuelType = fuelType.FuelType;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditGearbox(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}

            _carDataManager.EditGearboxType(model.GearboxId, model.Gearbox);
            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditGearbox(Guid GearboxTypeId)
        {
            // get gearbox type from database
            GearboxModel gearbox = _carDataManager.GetOneGearboxType(GearboxTypeId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.GearboxId = gearbox.Id;
            viewModel.Gearbox = gearbox.Gearbox;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditLocation(CarPropertiesViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(ShowCarProperties), model);
            //}

            _carDataManager.EditLocation(model.LocationId, model.City, model.Street, model.Number);
            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpGet]
        public IActionResult EditLocation(Guid LocationId)
        {
            // get location from database
            LocationModel location = _carDataManager.GetOneLocation(LocationId);
            CarPropertiesViewModel viewModel = new CarPropertiesViewModel();
            viewModel.LocationId = location.Id;
            viewModel.City = location.City;
            viewModel.Street = location.Street;
            viewModel.Number = location.Number;

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult DeleteBrand(Guid BrandId)
        {
            CarModel[] cars = _carDataManager.GetCarsByBrand(BrandId);

            if (cars.Any())
            {
                ModelState.AddModelError("BrandMessage", "Can't delete a brand with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteBrand(BrandId);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteModel(Guid CarModelId)
        {
            CarModel[] cars = _carDataManager.GetCarsByCarModel(CarModelId);

            if (cars.Any())
            {
                ModelState.AddModelError("CarModelMessage", "Can't delete a model with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteCarModel(CarModelId);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteFuelType(Guid FuelTypeId)
        {
            CarModel[] cars = _carDataManager.GetCarsByFuelType(FuelTypeId);

            if (cars.Any())
            {
                ModelState.AddModelError("FuelTypeMessage", "Can't delete a fuel type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteFuelType(FuelTypeId);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteGearbox(Guid GearboxTypeId)
        {
            CarModel[] cars = _carDataManager.GetCarsByGearboxType(GearboxTypeId);

            if (cars.Any())
            {
                ModelState.AddModelError("GearboxMessage", "Can't delete a gearbox type with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteGearboxType(GearboxTypeId);

            return RedirectToAction(nameof(ShowCarProperties));
        }

        [HttpPost]
        public IActionResult DeleteLocation(Guid LocationId)
        {
            CarModel[] cars = _carDataManager.GetCarsByLocation(LocationId);

            if (cars.Any())
            {
                ModelState.AddModelError("LocationMessage", "Can't delete a location with an existing car!");
                return RedirectToAction(nameof(ShowCarProperties));
            }

            _carDataManager.DeleteLocation(LocationId);

            return RedirectToAction(nameof(ShowCarProperties));
        }


        [HttpPost]
        public IActionResult AddFeedback(Guid CarId, string comment)
        {
            var user = _userManager.GetUserAsync(User).Result;
            _carDataManager.AddFeedback(CarId, comment, user);

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }

        [HttpPost]
        public IActionResult DeleteFeedback(Guid FeedbackId, Guid CarId)
        {
            _carDataManager.DeleteFeedback(FeedbackId);

            return RedirectToAction(nameof(OpenCar), new { CarId });
        }
    }
}
