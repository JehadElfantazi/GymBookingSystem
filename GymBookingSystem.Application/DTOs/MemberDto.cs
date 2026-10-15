using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Application.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }

    }
}
