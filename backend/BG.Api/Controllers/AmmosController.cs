using BG.App.DTOs.AmmoCrates;
using BG.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmmosController : ControllerBase
{
    private IAmmoService _ammoService;

    public AmmosController(IAmmoService ammoService)
    {
        _ammoService = ammoService;
    }

    [HttpPost]
    public async Task<ActionResult<AmmoResponse>> Create(CreateAmmoRequest request)
    {
        var response = await _ammoService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ammoService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] UpdateAmmoDetailsRequest request)
    {
        await _ammoService.UpdateDetailsAsync(id, request);
        return NoContent();
    }

    [HttpPost("issue")]
    public async Task<IActionResult> Issue(IssueAmmoRequest request)
    {
        await _ammoService.IssueAmmoAsync(request);
        return NoContent();
    }

    [HttpPost("restock")]
    public async Task<IActionResult> Restock(RestockAmmoRequest request)
    {
        await _ammoService.RestockAsync(request);
        return NoContent();
    }

    [HttpPost("audit")]
    public async Task<IActionResult> Audit(AuditAmmoInventoryRequest request)
    {
        await _ammoService.AuditInventoryAsync(request);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AmmoResponse>> GetById(Guid id)
    {
        var crate = await _ammoService.GetCrateByIdAsync(id);

        return Ok(crate);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AmmoResponse>>> GetAll()
    {
        var crates = await _ammoService.GetAllAsync();

        return Ok(crates);
    }
}