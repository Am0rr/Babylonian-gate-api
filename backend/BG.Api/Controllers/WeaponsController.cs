using BG.App.DTOs.Weapons;
using BG.App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BG.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeaponsController : ControllerBase
{
    private readonly IWeaponService _weaponService;

    public WeaponsController(IWeaponService weaponService)
    {
        _weaponService = weaponService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWeaponRequest request)
    {
        var id = await _weaponService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPatch("details")]
    public async Task<IActionResult> UpdateDetails(Guid id, [FromBody] UpdateWeaponDetailsRequest request)
    {
        await _weaponService.UpdateDetailsAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _weaponService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("issue")]
    public async Task<IActionResult> Issue(IssueWeaponRequest request)
    {
        await _weaponService.IssueWeaponAsync(request);
        return NoContent();
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnToStorage(ReturnWeaponToStorageRequest request)
    {
        await _weaponService.ReturnToStorageAsync(request);
        return NoContent();
    }

    [HttpPost("maintenance")]
    public async Task<IActionResult> SendToMaintenance(SendWeaponToMaintenanceRequest request)
    {
        await _weaponService.SendToMaintenanceAsync(request);
        return NoContent();
    }

    [HttpPost("report")]
    public async Task<IActionResult> ReportMissing(ReportWeaponMissingRequest request)
    {
        await _weaponService.ReportMissingAsync(request);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var weapon = await _weaponService.GetWeaponByIdAsync(id);

        return Ok(weapon);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var weapons = await _weaponService.GetAllAsync();

        return Ok(weapons);
    }
}