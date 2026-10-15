using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GymBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ApiControllerBase
{
    private readonly IBookingService _service;

    public BookingsController(IBookingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookingDto>>> GetBookings(int? memberId)
    {
        return Ok(await _service.GetAllAsync(memberId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value!.Id }, result.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var result = await _service.CancelAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }
}