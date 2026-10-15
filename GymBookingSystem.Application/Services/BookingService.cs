using System.Linq.Expressions;
using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Application.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;

    public BookingService(IApplicationDbContext context)
    {
        _context = context;
    }

    private static readonly Expression<Func<Booking, BookingDto>> ToDto = b => new BookingDto
    {
        Id = b.Id,
        MemberId = b.MemberId,
        MemberName = b.Member.FirstName + " " + b.Member.LastName,
        ClassScheduleId = b.ClassScheduleId,
        GymClassName = b.ClassSchedule.GymClass.Name,
        StartTime = b.ClassSchedule.StartTime,
        EndTime = b.ClassSchedule.EndTime,
        BookedAt = b.BookedAt
    };

    public async Task<List<BookingDto>> GetAllAsync(int? memberId)
    {
        var query = _context.Bookings.AsQueryable();

        if (memberId.HasValue)
            query = query.Where(b => b.MemberId == memberId.Value);

        return await query
            .OrderBy(b => b.ClassSchedule.StartTime)
            .Select(ToDto)
            .ToListAsync();
    }

    public async Task<ServiceResult<BookingDto>> GetByIdAsync(int id)
    {
        var dto = await _context.Bookings
            .Where(b => b.Id == id)
            .Select(ToDto)
            .FirstOrDefaultAsync();

        if (dto is null)
            return ServiceResult<BookingDto>.NotFound($"Booking with id {id} was not found.");

        return ServiceResult<BookingDto>.Success(dto);
    }

    public async Task<ServiceResult<BookingDto>> CreateAsync(CreateBookingDto dto)
    {
        // 1. Member must exist
        var memberExists = await _context.Members.AnyAsync(m => m.Id == dto.MemberId);
        if (!memberExists)
            return ServiceResult<BookingDto>.BadRequest($"Member with id {dto.MemberId} does not exist.");

        // 2. Schedule must exist (we also need its gym class for the capacity)
        var schedule = await _context.ClassSchedules
            .Include(s => s.GymClass)
            .FirstOrDefaultAsync(s => s.Id == dto.ClassScheduleId);
        if (schedule is null)
            return ServiceResult<BookingDto>.BadRequest($"Class schedule with id {dto.ClassScheduleId} does not exist.");

        // 3. Schedule must not have started
        if (schedule.StartTime <= DateTime.UtcNow)
            return ServiceResult<BookingDto>.Conflict("This class has already started and can no longer be booked.");

        // 4. No duplicate booking
        var alreadyBooked = await _context.Bookings
            .AnyAsync(b => b.MemberId == dto.MemberId && b.ClassScheduleId == dto.ClassScheduleId);
        if (alreadyBooked)
            return ServiceResult<BookingDto>.Conflict("This member is already booked on this class.");

        // 5. Schedule must not be full
        var bookedCount = await _context.Bookings
            .CountAsync(b => b.ClassScheduleId == dto.ClassScheduleId);
        if (bookedCount >= schedule.GymClass.Capacity)
            return ServiceResult<BookingDto>.Conflict("This class is full.");

        // 6. Create the booking
        var booking = new Booking
        {
            MemberId = dto.MemberId,
            ClassScheduleId = dto.ClassScheduleId,
            BookedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);

        // Touch the schedule so this save is rejected if another booking
        // changed the schedule (its RowVersion) after we read it
        _context.Entry(schedule).Property(s => s.StartTime).IsModified = true;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            // Must come first: it is a subclass of DbUpdateException
            return ServiceResult<BookingDto>.Conflict(
                "Someone else just booked this class. Please check availability and try again.");
        }
        catch (DbUpdateException)
        {
            // Only report "already booked" if that is really what happened
            var duplicate = await _context.Bookings
                .AnyAsync(b => b.MemberId == dto.MemberId && b.ClassScheduleId == dto.ClassScheduleId);

            if (duplicate)
                return ServiceResult<BookingDto>.Conflict("This member is already booked on this class.");

            throw;   // an unexpected database failure: let the global handler log it and return 500
        }


        var result = await _context.Bookings
            .Where(b => b.Id == booking.Id)
            .Select(ToDto)
            .FirstAsync();

        return ServiceResult<BookingDto>.Success(result);
    }

    public async Task<ServiceResult> CancelAsync(int id)
    {
        var booking = await _context.Bookings
            .Include(b => b.ClassSchedule)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking is null)
            return ServiceResult.NotFound($"Booking with id {id} was not found.");

        if (booking.ClassSchedule.StartTime <= DateTime.UtcNow)
            return ServiceResult.Conflict("Cannot cancel a booking for a class that has already started.");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return ServiceResult.Success();
    }
}
