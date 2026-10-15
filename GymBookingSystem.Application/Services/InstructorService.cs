using System.Linq.Expressions;
using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Application.Services;

public class InstructorService : IInstructorService
{
    private readonly IApplicationDbContext _context;

    public InstructorService(IApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Expression<Func<Instructor, InstructorDto>> ToDto = i => new InstructorDto
    {
        Id = i.Id,
        FirstName = i.FirstName,
        LastName = i.LastName,
        Email = i.Email
    };

    public async Task<List<InstructorDto>> GetAllAsync()
    {
        return await _context.Instructors.Select(ToDto).ToListAsync();
    }

    public async Task<ServiceResult<InstructorDto>> GetByIdAsync(int id)
    {
        var dto = await _context.Instructors
            .Where(i => i.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        if (dto is null)
            return ServiceResult<InstructorDto>.NotFound($"Instructor with id {id} was not found.");

        return ServiceResult<InstructorDto>.Success(dto);
    }

    public async Task<ServiceResult<InstructorDto>> CreateAsync(CreateInstructorDto dto)
    {
        var emailTaken = await _context.Instructors.AnyAsync(i => i.Email == dto.Email);
        if (emailTaken)
            return ServiceResult<InstructorDto>.Conflict($"An instructor with email '{dto.Email}' already exists.");

        var instructor = new Instructor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };

        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return ServiceResult<InstructorDto>.Success(new InstructorDto
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Email = instructor.Email
        });
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateInstructorDto dto)
    {
        var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == id);
        if (instructor is null)
            return ServiceResult.NotFound($"Instructor with id {id} was not found.");

        var emailTaken = await _context.Instructors.AnyAsync(i => i.Email == dto.Email && i.Id != id);
        if (emailTaken)
            return ServiceResult.Conflict($"An instructor with email '{dto.Email}' already exists.");

        instructor.FirstName = dto.FirstName;
        instructor.LastName = dto.LastName;
        instructor.Email = dto.Email;

        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var instructor = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == id);
        if (instructor is null)
            return ServiceResult.NotFound($"Instructor with id {id} was not found.");

        var hasClasses = await _context.GymClasses.AnyAsync(g => g.InstructorId == id);
        if (hasClasses)
            return ServiceResult.Conflict("Cannot delete an instructor who is assigned to gym classes.");

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();
        return ServiceResult.Success();
    }
}