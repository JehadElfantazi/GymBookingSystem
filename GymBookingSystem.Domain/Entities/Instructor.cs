namespace GymBookingSystem.Domain.Entities;

public class Instructor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<GymClass> GymClasses { get; set; } = [];
}