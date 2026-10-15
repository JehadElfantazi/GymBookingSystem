namespace GymBookingSystem.Application.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int ClassScheduleId { get; set; }
    public string GymClassName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime BookedAt { get; set; }
}