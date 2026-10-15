using System.Linq.Expressions;
using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Application.Services;

public class MemberService : IMemberService
{
    private readonly IApplicationDbContext _context;

    public MemberService(IApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Expression<Func<Member, MemberDto>> ToDto = m => new MemberDto
    {
        Id = m.Id,
        FirstName = m.FirstName,
        LastName = m.LastName,
        Email = m.Email,
        JoinedAt = m.JoinedAt
    };

    public async Task<List<MemberDto>> GetAllAsync()
    {
        return await _context.Members.Select(ToDto).ToListAsync();
    }

    public async Task<ServiceResult<MemberDto>> GetByIdAsync(int id)
    {
        var dto = await _context.Members
            .Where(m => m.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        if (dto is null)
            return ServiceResult<MemberDto>.NotFound($"Member with id {id} was not found.");

        return ServiceResult<MemberDto>.Success(dto);
    }

    public async Task<ServiceResult<MemberDto>> CreateAsync(CreateMemberDto dto)
    {
        var emailTaken = await _context.Members.AnyAsync(m => m.Email == dto.Email);
        if (emailTaken)
            return ServiceResult<MemberDto>.Conflict($"A member with email '{dto.Email}' already exists.");

        var member = new Member
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            JoinedAt = DateTime.UtcNow
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        return ServiceResult<MemberDto>.Success(new MemberDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            JoinedAt = member.JoinedAt
        });
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateMemberDto dto)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        if (member is null)
            return ServiceResult.NotFound($"Member with id {id} was not found.");

        var emailTaken = await _context.Members.AnyAsync(m => m.Email == dto.Email && m.Id != id);
        if (emailTaken)
            return ServiceResult.Conflict($"A member with email '{dto.Email}' already exists.");

        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Email = dto.Email;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        if (member is null)
            return ServiceResult.NotFound($"Member with id {id} was not found.");

        var hasBookings = await _context.Bookings.AnyAsync(b => b.MemberId == id);
        if (hasBookings)
            return ServiceResult.Conflict("Cannot delete a member who has bookings.");

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }
}