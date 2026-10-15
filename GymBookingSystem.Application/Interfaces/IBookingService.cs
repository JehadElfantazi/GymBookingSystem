using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;

namespace GymBookingSystem.Application.Interfaces;

public interface IBookingService
{
    Task<List<BookingDto>> GetAllAsync(int? memberId);
    Task<ServiceResult<BookingDto>> GetByIdAsync(int id);
    Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto);
    Task<ServiceResult> CancelAsync(int id);
}