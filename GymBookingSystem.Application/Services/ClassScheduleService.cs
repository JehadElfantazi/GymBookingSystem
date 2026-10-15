using System.Linq.Expressions;
using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Application.Services;

public class ClassScheduleService : IClassScheduleService
{
    private readonly IApplicationDbContext _context;

    public ClassScheduleService(IApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Expression<Func<ClassSchedule, ClassScheduleDto>> ToDto = s => new ClassScheduleDto
    {
        Id = s.Id,
        GymClassId = s.GymClassId,
        GymClassName = s.GymClass.Name,
        StartTime = s.StartTime,
        EndTime = s.EndTime,
        Capacity = s.GymClass.Capacity,
        BookedCount = s.Bookings.Count,
        AvailableSpots = s.GymClass.Capacity - s.Bookings.Count
    };

    // Returns an error message, or null when the times are valid
    private static string? ValidateTimes(DateTime start, DateTime end)
    {
        if (end <= start)
            return "End time must be after start time.";

        if (start <= DateTime.UtcNow)
            return "Start time must be in the future.";

        return null;
    }

    public async Task<List<ClassScheduleDto>> GetAllAsync(bool upcomingOnly)
    {
        var query = _context.ClassSchedules.AsQueryable();

        if (upcomingOnly)
            query = query.Where(s => s.StartTime > DateTime.UtcNow);

        return await query
            .OrderBy(s => s.StartTime)
            .Select(ToDto)
            .ToListAsync();
    }

    public async Task<ServiceResult<ClassScheduleDto>> GetByIdAsync(int id)
    {
        var dto = await _context.ClassSchedules
            .Where(s => s.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        if (dto is null)
            return ServiceResult<ClassScheduleDto>.NotFound($"Class schedule with id {id} was not found.");

        return ServiceResult<ClassScheduleDto>.Success(dto);
    }

    public async Task<ServiceResult<ClassScheduleDto>> CreateAsync(CreateClassScheduleDto dto)
    {
        var gymClassExists = await _context.GymClasses.AnyAsync(g => g.Id == dto.GymClassId);
        if (!gymClassExists)
            return ServiceResult<ClassScheduleDto>.BadRequest($"Gym class with id {dto.GymClassId} does not exist.");

        var timeError = ValidateTimes(dto.StartTime, dto.EndTime);
        if (timeError is not null)
            return ServiceResult<ClassScheduleDto>.BadRequest(timeError);

        var schedule = new ClassSchedule
        {
            GymClassId = dto.GymClassId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        _context.ClassSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        // Reuse the one projection instead of building the DTO by hand
        var result = await _context.ClassSchedules
            .Where(s => s.Id == schedule.Id)
            .Select(ToDto)
            .FirstAsync();

        return ServiceResult<ClassScheduleDto>.Success(result);
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateClassScheduleDto dto)
    {
        var schedule = await _context.ClassSchedules.FirstOrDefaultAsync(s => s.Id == id);
        if (schedule is null)
            return ServiceResult.NotFound($"Class schedule with id {id} was not found.");

        if (schedule.StartTime <= DateTime.UtcNow)
            return ServiceResult.Conflict("Cannot modify a schedule that has already started.");

        var timeError = ValidateTimes(dto.StartTime, dto.EndTime);
        if (timeError is not null)
            return ServiceResult.BadRequest(timeError);

        schedule.StartTime = dto.StartTime;
        schedule.EndTime = dto.EndTime;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var schedule = await _context.ClassSchedules.FirstOrDefaultAsync(s => s.Id == id);
        if (schedule is null)
            return ServiceResult.NotFound($"Class schedule with id {id} was not found.");

        var hasBookings = await _context.Bookings.AnyAsync(b => b.ClassScheduleId == id);
        if (hasBookings)
            return ServiceResult.Conflict("Cannot delete a schedule that has bookings.");

        _context.ClassSchedules.Remove(schedule);
        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }
}
