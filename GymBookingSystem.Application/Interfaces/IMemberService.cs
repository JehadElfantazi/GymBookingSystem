using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;

namespace GymBookingSystem.Application.Interfaces;

public interface IMemberService
{
    Task<List<MemberDto>> GetAllAsync();
    Task<ServiceResult<MemberDto>> GetByIdAsync(int id);
    Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto);
    Task<ServiceResult> UpdateAsync(int id, UpdateMemberDto dto);
    Task<ServiceResult> DeleteAsync(int id);
}