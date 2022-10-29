namespace VehicleRentalSystem.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }

        public ICollection<ReservationModel> Reservations { get; set; }
        public ICollection<FeedbackModel> Feedbacks { get; set; }

        public IList<string> Roles { get; set; }
        public string RolesAsString
        {
            get
            {
                return string.Join(", ", Roles);
            }
        }
    }
}