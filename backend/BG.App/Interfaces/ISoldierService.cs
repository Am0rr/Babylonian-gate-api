using BG.App.DTOs.Soldiers;

namespace BG.App.Interfaces;

public interface ISoldierService
{
    Task<Guid> CreateAsync(CreateSoldierRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateSoldierRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid soldierId, CancellationToken cancellationToken = default);
    Task<SoldierResponse?> GetSoldierByIdAsync(Guid soldierId, CancellationToken cancellationToken = default);
    Task<List<SoldierResponse>> GetAllAsync(CancellationToken cancellationToken = default);
}