using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class AdminViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel[] Users { get; set; }


        [Required(ErrorMessage = "The field New Role is required.")]
        public string NewRole { get; set; }

        [Required(ErrorMessage = "The field User is required.")]
        public string SelectedUser { get; set; }

        [Required(ErrorMessage = "The field Role is required.")]
        public string SelectedRole { get; set; }

        public SelectList UserDropdown { get; set; }
        public SelectList RoleDropdown { get; set; }
    }
}
