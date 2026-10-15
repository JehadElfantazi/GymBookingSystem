using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Application.DTOs
{
    public class UpdateClassScheduleDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
