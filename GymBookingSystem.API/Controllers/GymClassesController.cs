using GymBookingSystem.Application.Common;
using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace GymBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GymClassesController : ApiControllerBase
{
    private readonly IGymClassService _service;

    public GymClassesController(IGymClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<GymClassDto>>> GetGymClasses()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GymClassDto>> GetGymClass(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<GymClassDto>> CreateGymClass(CreateGymClassDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return CreatedAtAction(nameof(GetGymClass), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGymClass(int id, UpdateGymClassDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGymClass(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }
}