using System.ComponentModel.DataAnnotations;

namespace GymBookingSystem.Application.DTOs;

public class CreateBookingDto
{
    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }

    [Range(1, int.MaxValue)]
    public int ClassScheduleId { get; set; }
}