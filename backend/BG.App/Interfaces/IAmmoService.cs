using BG.App.DTOs;
using BG.Domain.Entities;

namespace BG.App.Interfaces;

public interface IAmmoService
{
    Task<Guid> CreateAsync(CreateAmmoRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid crateId, CancellationToken cancellationToken = default);
    Task UpdateDetailsAsync(UpdateAmmoDetailsRequest request, CancellationToken cancellationToken = default);
    Task IssueAmmoAsync(IssueAmmoRequest request, CancellationToken cancellationToken = default);
    Task RestockAsync(RestockAmmoRequest request, CancellationToken cancellationToken = default);
    Task AuditInventoryAsync(AuditAmmoInventoryRequest request, CancellationToken cancellationToken = default);
    Task<AmmoResponse?> GetCrateByIdAsync(Guid crateId, CancellationToken cancellationToken = default);
    Task<List<AmmoResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}