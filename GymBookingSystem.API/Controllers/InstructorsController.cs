using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ApiControllerBase
{
    private readonly IInstructorService _service;

    public InstructorsController(IInstructorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<InstructorDto>>> GetInstructors()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<InstructorDto>> GetInstructor(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<InstructorDto>> CreateInstructor(CreateInstructorDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return CreatedAtAction(nameof(GetInstructor), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateInstructor(int id, UpdateInstructorDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }
}