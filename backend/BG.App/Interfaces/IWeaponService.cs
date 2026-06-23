using BG.App.DTOs.Weapons;

namespace BG.App.Interfaces;

public interface IWeaponService
{
    Task<Guid> CreateAsync(CreateWeaponRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid weaponId, CancellationToken cancellationToken = default);
    Task UpdateDetailsAsync(UpdateWeaponDetailsRequest request, CancellationToken cancellationToken = default);
    Task IssueWeaponAsync(IssueWeaponRequest request, CancellationToken cancellationToken = default);
    Task ReturnToStorageAsync(ReturnWeaponToStorageRequest request, CancellationToken cancellationToken = default);
    Task SendToMaintenanceAsync(SendWeaponToMaintenanceRequest request, CancellationToken cancellationToken = default);
    Task ReportMissingAsync(ReportWeaponMissingRequest request, CancellationToken cancellationToken = default);
    Task<WeaponResponse> GetWeaponByIdAsync(Guid weaponId, CancellationToken cancellationToken = default);
    Task<IEnumerable<WeaponResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}