using BG.App.DTOs.Soldiers;

namespace BG.App.Interfaces;

public interface ISoldierService
{
    Task<SoldierResponse> CreateAsync(CreateSoldierRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid soldierId, UpdateSoldierRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid soldierId, CancellationToken cancellationToken = default);
    Task<SoldierResponse> GetSoldierByIdAsync(Guid soldierId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SoldierResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}