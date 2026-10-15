using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;

namespace GymBookingSystem.Application.Interfaces;

public interface IClassScheduleService
{
    Task<List<ClassScheduleDto>> GetAllAsync(bool upcomingOnly);
    Task<ServiceResult<ClassScheduleDto>> GetByIdAsync(int id);
    Task<ServiceResult<ClassScheduleDto>> CreateAsync(CreateClassScheduleDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateClassScheduleDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}
