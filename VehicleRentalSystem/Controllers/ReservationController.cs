using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    public class ReservationController : Controller
    {
        private readonly CarDataManager _carDataManager;
        private readonly UserManager<UserModel> _userManager;

        public ReservationController(CarDataManager carDataManager, UserManager<UserModel> userManager)
        {
            _carDataManager = carDataManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Confirmation(ReservationViewModel viewModel)
        {
            var userId = _userManager.GetUserId(User);

            if (viewModel == null)
            {
                viewModel = new ReservationViewModel();
            }

            if (viewModel.CarId == Guid.Empty)
            {
                return RedirectToAction(nameof(AddReservation), viewModel);
            }

            CarModel car = _carDataManager.GetOneCar(viewModel.CarId);
            viewModel.CarId = car.Id;

            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;

            PaymentModel payment = _carDataManager.GetPayment(viewModel.PaymentId);
            ReservationModel reservation = _carDataManager.GetOneReservation(viewModel.ReservationId);

            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = payment.Amount;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationViewModel model)
        {
            //nav valid - nevar saglabat
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(AddReservation), model);
            //}

            if(model.StartDate < DateTime.Now || model.StartDate > model.EndDate || model.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("IncorrectDates", "Please input correct dates");
                return View(nameof(AddReservation), model);
            }

            //if (!_reservationValidator.AreDatesValid(model.CarId, model.StartDate, model.EndDate))
            //{
            //    ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
            //    return View(nameof(AddReservation), model);
            //}

            CarModel[] cars = _carDataManager.CheckIfDatesValid(model.CarId, model.StartDate, model.EndDate);
            if (cars.Any())
            {
                ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
                return View(nameof(AddReservation), model);
            }

            var user = await _userManager.GetUserAsync(User);

            CarModel carModel = _carDataManager.GetOneCar(model.CarId);

            TimeSpan timeSpan = model.EndDate - model.StartDate;
            double days = timeSpan.TotalDays;
            double amount = carModel.DailyPrice * days;

            model.PaymentId = _carDataManager.AddPayment(amount, model.StartDate, model.EndDate, carModel.Id);
            PaymentModel payment = _carDataManager.GetPayment(model.PaymentId);

            model.ReservationId = _carDataManager.AddReservation(model.StartDate, model.EndDate, carModel.Id, user, payment);
            model.Amount = amount;

            model.SuccessMessage = "Reservation successful!";

            return RedirectToAction(nameof(Confirmation), model);
        }

        [HttpGet]
        public IActionResult AddReservation(Guid carId)
        {
            // get car from database (CarModel)
            CarModel car = _carDataManager.GetOneCar(carId);

            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.CarId = car.Id;

            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;

            return View(viewModel);
        }
    }
}
