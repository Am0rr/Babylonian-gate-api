using BG.App.DTOs.AmmoCrates;
using BG.App.Interfaces;
using BG.Domain.Interfaces;
using BG.Domain.Enums;
using BG.Domain.Entities;
using AutoMapper;

namespace BG.App.Services;

public class AmmoService : BaseService, IAmmoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AmmoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AmmoResponse> CreateAsync(CreateAmmoRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var type = Enum.Parse<AmmoType>(request.Type, ignoreCase: true);

        var crate = new AmmoCrate(
            request.LotNumber,
            request.Caliber,
            request.Quantity,
            type
        );

        _unitOfWork.Crates.Add(crate);

        var log = OperationLog.Create("Create", $"Registered crate Lot #{crate.LotNumber} ({crate.Caliber}, {type})", crate.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AmmoResponse>(crate);
    }

    public async Task DeleteAsync(Guid crateId, CancellationToken cancellationToken)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {crateId} not found.");

        if (crate.Quantity > 0)
        {
            throw new InvalidOperationException("Cannot delete crate containing ammo. Issue or empty it first.");
        }

        _unitOfWork.Crates.Delete(crate);

        var log = OperationLog.Create("Delete", $"Deleted crate Lot #{crate.LotNumber} ({crate.Caliber})", crate.Id);
        _unitOfWork.Logs.Add(log);


        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDetailsAsync(Guid crateId, UpdateAmmoDetailsRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {crateId} not found.");

        var logDetails = new List<string>();

        if (request.LotNumber != null)
        {
            string oldLotNumber = crate.LotNumber;

            crate.ChangeLotNumber(request.LotNumber);

            logDetails.Add($"Lot: '{oldLotNumber}' -> '{request.LotNumber}'");
        }

        if (request.Caliber != null)
        {
            string oldCal = crate.Caliber;

            crate.ChangeCaliber(request.Caliber);

            logDetails.Add($"Caliber: '{oldCal}' -> '{request.Caliber}'");
        }

        if (request.Type != null)
        {
            string oldType = crate.Type.ToString();

            var type = Enum.Parse<AmmoType>(request.Type, ignoreCase: true);
            crate.ChangeType(type);

            logDetails.Add($"Type: '{oldType}' -> '{request.Type}'");
        }

        var log = OperationLog.Create("Update", $"Corrected details: {string.Join(", ", logDetails)}", crate.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task IssueAmmoAsync(IssueAmmoRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {request.SoldierId} not found.");

        crate.Issue(request.Amount);

        _unitOfWork.Crates.Update(crate);

        string logMessage = $"Issued {request.Amount} rounds ({crate.Type}) to {soldier.LastName} {soldier.FirstName}. " +
                        $"From Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

        var log = OperationLog.Create("Issue", logMessage, crate.Id);

        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RestockAsync(RestockAmmoRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {request.SoldierId} not found.");


        crate.Restock(request.Amount);

        string logMessage = $"Restocked {request.Amount} rounds ({crate.Type}) from {soldier.LastName} {soldier.FirstName}. " +
                        $"In Lot #{crate.LotNumber}. Remaining: {crate.Quantity}";

        var log = OperationLog.Create("Restock", logMessage, crate.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task AuditInventoryAsync(AuditAmmoInventoryRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var crate = await _unitOfWork.Crates.GetByIdAsync(request.CrateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {request.CrateId} not found.");

        int diff = request.ActualQuantity - crate.Quantity;

        if (diff == 0) return;

        crate.AdjustQuantity(request.ActualQuantity);

        string diffSign = diff > 0 ? "+" : "";
        var log = OperationLog.Create("Audit", $"Inventory Check. Correction: {diffSign}{diff}. New Balance: {crate.Quantity}", crate.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AmmoResponse> GetCrateByIdAsync(Guid crateId, CancellationToken cancellationToken)
    {
        var crate = await _unitOfWork.Crates.GetByIdAsync(crateId, cancellationToken)
            ?? throw new KeyNotFoundException($"Crate with ID {crateId} not found.");

        return _mapper.Map<AmmoResponse>(crate);
    }

    public async Task<IEnumerable<AmmoResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var crates = await _unitOfWork.Crates.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<AmmoResponse>>(crates);
    }
}