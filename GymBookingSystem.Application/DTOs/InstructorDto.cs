using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Application.DTOs
{
    public class InstructorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


    }
}
