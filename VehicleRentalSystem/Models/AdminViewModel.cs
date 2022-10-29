using Microsoft.AspNetCore.Mvc.Rendering;

namespace VehicleRentalSystem.Models
{
    public class AdminViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel[] Users { get; set; }

        public string NewRole { get; set; }

        public string SelectedUser { get; set; }
        public string SelectedRole { get; set; }

        public SelectList UserDropdown { get; set; }
        public SelectList RoleDropdown { get; set; }
    }
}
