using BG.App.DTOs.Soldiers;
using BG.App.Interfaces;
using BG.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoldiersController : ControllerBase
{
    private readonly ISoldierService _soldierService;

    public SoldiersController(ISoldierService soldierService)
    {
        _soldierService = soldierService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSoldierRequest request)
    {
        var id = await _soldierService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPatch]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSoldierRequest request)
    {
        await _soldierService.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _soldierService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var soldier = await _soldierService.GetSoldierByIdAsync(id);

        return Ok(soldier);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var soldiers = await _soldierService.GetAllAsync();
        return Ok(soldiers);
    }
}