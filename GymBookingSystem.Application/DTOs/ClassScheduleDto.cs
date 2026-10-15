namespace GymBookingSystem.Application.DTOs;

public class ClassScheduleDto
{
    public int Id { get; set; }
    public int GymClassId { get; set; }
    public string GymClassName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public int BookedCount { get; set; }
    public int AvailableSpots { get; set; }
}