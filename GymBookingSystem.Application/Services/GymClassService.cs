using System.Linq.Expressions;
using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Application.Services;

public class GymClassService : IGymClassService
{
    private readonly IApplicationDbContext _context;

    public GymClassService(IApplicationDbContext context)
    {
        _context = context;
    }


    private static readonly Expression<Func<GymClass, GymClassDto>> ToDto = g => new GymClassDto
    {
        Id = g.Id,
        Name = g.Name,
        Description = g.Description,
        Capacity = g.Capacity,
        InstructorId = g.InstructorId,
        InstructorName = g.Instructor.FirstName + " " + g.Instructor.LastName
    };

    public async Task<List<GymClassDto>> GetAllAsync()
    {
        return await _context.GymClasses.Select(ToDto).ToListAsync();
    }

    public async Task<ServiceResult<GymClassDto>> GetByIdAsync(int id)
    {
        var dto = await _context.GymClasses
            .Where(g => g.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        if (dto is null)
            return ServiceResult<GymClassDto>.NotFound($"Gym class with id {id} was not found.");

        return ServiceResult<GymClassDto>.Success(dto);
    }

    public async Task<ServiceResult<GymClassDto>> CreateAsync(CreateGymClassDto dto)
    {
        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(i => i.Id == dto.InstructorId);

        if (instructor is null)
            return ServiceResult<GymClassDto>.BadRequest($"Instructor with id {dto.InstructorId} does not exist.");

        var gymClass = new GymClass
        {
            Name = dto.Name,
            Description = dto.Description,
            Capacity = dto.Capacity,
            InstructorId = dto.InstructorId
        };

        _context.GymClasses.Add(gymClass);
        await _context.SaveChangesAsync();

        return ServiceResult<GymClassDto>.Success(new GymClassDto
        {
            Id = gymClass.Id,
            Name = gymClass.Name,
            Description = gymClass.Description,
            Capacity = gymClass.Capacity,
            InstructorId = gymClass.InstructorId,
            InstructorName = $"{instructor.FirstName} {instructor.LastName}"
        });
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateGymClassDto dto)
    {
        var gymClass = await _context.GymClasses.FirstOrDefaultAsync(g => g.Id == id);
        if (gymClass is null)
            return ServiceResult.NotFound($"Gym class with id {id} was not found.");

        var instructorExists = await _context.Instructors.AnyAsync(i => i.Id == dto.InstructorId);
        if (!instructorExists)
            return ServiceResult.BadRequest($"Instructor with id {dto.InstructorId} does not exist.");

        gymClass.Name = dto.Name;
        gymClass.Description = dto.Description;
        gymClass.Capacity = dto.Capacity;
        gymClass.InstructorId = dto.InstructorId;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var gymClass = await _context.GymClasses.FirstOrDefaultAsync(g => g.Id == id);
        if (gymClass is null)
            return ServiceResult.NotFound($"Gym class with id {id} was not found.");

        var hasSchedules = await _context.ClassSchedules.AnyAsync(cs => cs.GymClassId == id);
        if (hasSchedules)
            return ServiceResult.Conflict("Cannot delete a gym class that has schedules. Delete its schedules first.");

        _context.GymClasses.Remove(gymClass);
        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }
}