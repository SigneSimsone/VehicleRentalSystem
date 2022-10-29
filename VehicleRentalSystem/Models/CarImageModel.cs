﻿using System.ComponentModel.DataAnnotations;

namespace VehicleRentalSystem.Models
{
    public class CarImageModel
    {
        [Key]
        public Guid Id { get; set; }

        public string ImagePath { get; set; }

        public CarImageModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
