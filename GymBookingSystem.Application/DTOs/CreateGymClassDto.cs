using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GymBookingSystem.Application.DTOs
{
    public class CreateGymClassDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(1,100)]
        public int Capacity { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int InstructorId { get; set; }

    }
}
