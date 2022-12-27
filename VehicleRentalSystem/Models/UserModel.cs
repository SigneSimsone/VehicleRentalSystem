using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VehicleRentalSystem.Models
{
    public class UserModel : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string DriversLicenseNr { get; set; }

        public ICollection<ReservationModel> Reservations { get; set; }
        public ICollection<FeedbackModel> Feedbacks { get; set; }
    }
}
