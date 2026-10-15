using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;

namespace GymBookingSystem.Application.Interfaces;

public interface IInstructorService
{
    Task<List<InstructorDto>> GetAllAsync();
    Task<ServiceResult<InstructorDto>> GetByIdAsync(int id);
    Task<ServiceResult<InstructorDto>> CreateAsync(CreateInstructorDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateInstructorDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}