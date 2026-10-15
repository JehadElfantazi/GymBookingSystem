using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;

namespace GymBookingSystem.Application.Interfaces;

public interface IGymClassService
{
    Task<List<GymClassDto>> GetAllAsync();
    Task<ServiceResult<GymClassDto>> GetByIdAsync(int id);
    Task<ServiceResult<GymClassDto>> CreateAsync(CreateGymClassDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateGymClassDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}