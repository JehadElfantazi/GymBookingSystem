using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymBookingSystem.Application.DTOs
{
    public class CreateClassScheduleDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "GymClassId must be greater than 0.")]
        public int GymClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
