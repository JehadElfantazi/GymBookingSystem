using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime BookedAt { get; set; }
        public int MemberId { get; set; }          // foreign key
        public Member Member { get; set; } = null!;   // navigation

        public int ClassScheduleId { get; set; }          // foreign key
        public ClassSchedule ClassSchedule { get; set; } = null!;   // navigation



    }
}
