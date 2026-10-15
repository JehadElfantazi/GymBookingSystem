using GymBookingSystem.Application.DTOs;
using GymBookingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymBookingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ApiControllerBase
{
    private readonly IMemberService _service;

    public MembersController(IMemberService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<MemberDto>>> GetMembers()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberDto>> GetMember(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember(CreateMemberDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return CreatedAtAction(nameof(GetMember), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMember(int id, UpdateMemberDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return ToErrorResult(result);

        return NoContent();
    }
}