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
            viewModel.PaymentDateString = payment.Date.ToString();

            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

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

            if (model.StartDate < DateTime.Now || model.StartDate > model.EndDate || model.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("IncorrectDates", "Please input correct dates");
                return View(nameof(AddReservation), model);
            }

            //if (!_reservationValidator.AreDatesValid(model.CarId, model.StartDate, model.EndDate))
            //{
            //    ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
            //    return View(nameof(AddReservation), model);
            //}

            ReservationModel[] reservations = _carDataManager.CheckIfDatesValid(model.CarId, model.StartDate, model.EndDate);
            if (reservations.Any())
            {
                ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
                return View(nameof(AddReservation), model);
            }

            var user = await _userManager.GetUserAsync(User);

            CarModel car = _carDataManager.GetOneCar(model.CarId);

            TimeSpan timeSpan = model.EndDate - model.StartDate;
            double days = timeSpan.TotalDays;
            double amount = car.DailyPrice * days;

            model.PaymentId = _carDataManager.AddPayment(amount);
            PaymentModel payment = _carDataManager.GetPayment(model.PaymentId);

            model.ReservationId = _carDataManager.AddReservation(model.StartDate, model.EndDate, car.Id, user, payment);
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


        public IActionResult UserReservationHistory()
        {
            var user = _userManager.GetUserAsync(User).Result;

            ReservationViewModel viewModel = new ReservationViewModel();
            var reservations = _carDataManager.GetUserReservations(user);

            viewModel.Reservations = reservations;
            viewModel.UserId = user.Id;

            return View("UserReservationHistory", viewModel);
        }

        public IActionResult AllReservations()
        {
            var user = _userManager.GetUserAsync(User).Result;

            ReservationViewModel viewModel = new ReservationViewModel();
            var reservations = _carDataManager.GetReservations();
            CarModel[] car = _carDataManager.GetCars();

            viewModel.Reservations = reservations;
            viewModel.UserId = user.Id;
            viewModel.Email = user.Email;

            return View("AllReservations", viewModel);
        }

        [HttpPost]
        public IActionResult AddPaymentDate(DateTime PaymentDate, Guid ReservationId, ReservationViewModel model)
        {
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            _carDataManager.AddPaymentDate(reservation.PaymentId, PaymentDate);

            return RedirectToAction(nameof(AllReservations));
        }

        [HttpGet]
        public IActionResult AddPaymentDate(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(AllReservations));
            }

            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.ReservationId = ReservationId;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Email = reservation.User.Email;

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult OpenReservation(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(UserReservationHistory));
            }

            // get reservation from database
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

            return View(nameof(Confirmation), viewModel);
        }

        [HttpGet]
        public IActionResult OpenReservationAdmin(Guid ReservationId)
        {
            if (ReservationId == Guid.Empty)
            {
                return RedirectToAction(nameof(AllReservations));
            }

            // get reservation from database
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

            ReservationViewModel viewModel = new ReservationViewModel();

            viewModel.CarId = car.Id;
            viewModel.Brand = car.Brand.Brand;
            viewModel.CarModel = car.Model.Model;
            viewModel.FuelType = car.FuelType.FuelType;
            viewModel.GearboxType = car.GearboxType.Gearbox;
            viewModel.Location = car.Location.FullLocation;
            viewModel.DailyPrice = car.DailyPrice;
            viewModel.StartDateString = reservation.StartDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.EndDateString = reservation.EndDate.ToString("dd/MM/yyyy HH:mm");
            viewModel.Amount = reservation.Payment.Amount;
            viewModel.PaymentDateString = reservation.Payment.Date.ToString();
            viewModel.Name = reservation.User.Name;
            viewModel.Surname = reservation.User.Surname;
            viewModel.Email = reservation.User.Email;
            viewModel.PhoneNr = reservation.User.PhoneNumber;

            return View(nameof(Confirmation), viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditReservation(Guid ReservationId, ReservationViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToAction(nameof(Confirmation), model);
            //}

            if (model.StartDate < DateTime.Now || model.StartDate > model.EndDate || model.EndDate < DateTime.Now)
            {
                ModelState.AddModelError("IncorrectDates", "Please input correct dates");
                return View(nameof(EditReservation), model);
            }

            ReservationModel[] reservations = _carDataManager.CheckIfDatesValid(model.CarId, model.StartDate, model.EndDate);
            if (reservations.Any())
            {
                ModelState.AddModelError("CarNotAvailable", "this car not available for this period");
                return View(nameof(EditReservation), model);
            }

            var user = await _userManager.GetUserAsync(User);
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(model.CarId);
            PaymentModel payment = _carDataManager.GetPayment(reservation.PaymentId);

            TimeSpan timeSpan = model.EndDate - model.StartDate;
            double days = timeSpan.TotalDays;
            double amount = car.DailyPrice * days;
            model.Amount = amount;
            model.PaymentId = reservation.PaymentId;

            _carDataManager.EditPayment(payment.Id, amount);
            _carDataManager.EditReservation(ReservationId, model.StartDate, model.EndDate, car.Id, user, payment);

            model.SuccessMessage = "Reservation successful!";
            return RedirectToAction(nameof(Confirmation), model);
        }

        [HttpGet]
        public IActionResult EditReservation(Guid ReservationId)
        {
            ReservationModel reservation = _carDataManager.GetOneReservation(ReservationId);
            CarModel car = _carDataManager.GetOneCar(reservation.Car.Id);

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

        [HttpPost]
        public IActionResult DeleteReservation(Guid ReservationId)
        {
            _carDataManager.DeleteReservation(ReservationId);

            return RedirectToAction(nameof(UserReservationHistory));
        }

        [HttpPost]
        public IActionResult DeleteReservationAdmin(Guid ReservationId)
        {
            _carDataManager.DeleteReservation(ReservationId);

            return RedirectToAction(nameof(AllReservations));
        }
    }
}
