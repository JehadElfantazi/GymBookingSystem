namespace GymBookingSystem.Domain.Entities;

public class GymClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }

    public int InstructorId { get; set; }          // foreign key
    public Instructor Instructor { get; set; } = null!;   // navigation

    public ICollection<ClassSchedule> ClassSchedules { get; set; } = [];
}