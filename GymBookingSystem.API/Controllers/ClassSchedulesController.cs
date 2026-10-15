using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassSchedulesController : ApiControllerBase
{
    private readonly IClassScheduleService _service;

    public ClassSchedulesController(IClassScheduleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClassScheduleDto>>> GetClassSchedules(bool upcomingOnly = false)
    {
        return Ok(await _service.GetAllAsync(upcomingOnly));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClassScheduleDto>> GetClassSchedule(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<ClassScheduleDto>> CreateClassSchedule(CreateClassScheduleDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return CreatedAtAction(nameof(GetClassSchedule), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateClassSchedule(int id, UpdateClassScheduleDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteClassSchedule(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }
}
