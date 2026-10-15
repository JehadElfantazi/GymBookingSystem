namespace GymBookingSystem.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }   // stored in UTC

    public ICollection<Booking> Bookings { get; set; } = [];


}