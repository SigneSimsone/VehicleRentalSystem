using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Controllers
{
    internal class _reservationValidator
    {
        private readonly CarDataManager _carDataManager;

        public _reservationValidator(CarDataManager carDataManager)
        {
            _carDataManager = carDataManager;
        }
        internal bool AreDatesValid(Guid carId, DateTime requestedStartDate, DateTime requestedEndDate)
        {
            ReservationModel[] reservations = _carDataManager.CheckIfDatesValid(carId, requestedStartDate, requestedEndDate);
            //Guid[] carIdList = cars.Select(x => x.Id).ToArray();
            if(!reservations.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}