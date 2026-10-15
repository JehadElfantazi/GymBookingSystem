using System;
using System.Collections.Generic;
using System.Text;

namespace GymBookingSystem.Domain.Entities
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte[] RowVersion { get; set; } = [];

        public int GymClassId { get; set; }          // foreign key
        public GymClass GymClass { get; set; } = null!;   // navigation

        public ICollection<Booking> Bookings { get; set; } = [];

    }
}
