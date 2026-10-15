using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Application.DTOs
{
    public class GymClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = string.Empty;
    }
}
