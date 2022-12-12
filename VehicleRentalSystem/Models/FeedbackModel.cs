using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class FeedbackModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public CarModel Car { get; set; }
        public UserModel User { get; set; }

        public FeedbackModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
