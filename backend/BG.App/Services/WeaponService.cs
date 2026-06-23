using BG.App.DTOs;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Enums;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class WeaponService : BaseService, IWeaponService
{
    private readonly IUnitOfWork _unitOfWork;

    public WeaponService(
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CreateWeaponRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var type = Enum.Parse<WeaponType>(request.Type, ignoreCase: true);

        var weapon = new Weapon(
            request.CodeName,
            request.SerialNumber,
            request.Caliber,
            type
        );

        _unitOfWork.Weapons.Add(weapon);

        var log = OperationLog.Create("Create", $"Claimed weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return weapon.Id;
    }

    public async Task DeleteAsync(Guid weaponId, CancellationToken cancellationToken)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");

        if (weapon.Status == WeaponStatus.Deployed)
            throw new InvalidOperationException("Cannot delete weapon because it is currently issued to a soldier. Return it to storage first.");

        _unitOfWork.Weapons.Delete(weapon);

        var log = OperationLog.Create("Delete", $"Deleted weapon {weapon.Codename}, with SN {weapon.SerialNumber}", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDetailsAsync(UpdateWeaponDetailsRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {request.Id} not found.");

        bool hasChanges = false;
        var logDetails = new List<string>();

        if (request.Codename != weapon.Codename)
        {
            string oldCodeName = weapon.Codename;
            weapon.ChangeCodeName(request.Codename);
            logDetails.Add($"Codename: '{oldCodeName}' -> '{request.Codename}'");
            hasChanges = true;
        }

        if (request.SerialNumber != weapon.SerialNumber)
        {
            string oldSerialNumber = weapon.SerialNumber;
            weapon.CorrectSerialNumber(request.SerialNumber);
            logDetails.Add($"Serial Number: '{oldSerialNumber}' -> '{request.SerialNumber}'");
            hasChanges = true;
        }

        if (request.Caliber != weapon.Caliber)
        {
            string oldCaliber = weapon.Caliber;
            weapon.CorrectCaliber(request.Caliber);
            logDetails.Add($"Caliber: '{oldCaliber}' -> '{request.Caliber}'");
            hasChanges = true;
        }

        if (!hasChanges)
        {
            return;
        }

        var log = OperationLog.Create("Update", $"Updated details: {string.Join(", ", logDetails)}", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task IssueWeaponAsync(IssueWeaponRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.WeaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {request.WeaponId} not found.");

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.SoldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {request.SoldierId} not found.");

        weapon.IssueTo(soldier.Id);

        var log = OperationLog.Create("Issue", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber} \n - Issued to {soldier.LastName} {soldier.FirstName} (ID: {soldier.Id})", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReturnToStorageAsync(ReturnWeaponToStorageRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.WeaponId)
            ?? throw new KeyNotFoundException($"Weapon with ID {request.WeaponId} not found.");

        weapon.ApplyWear(request.RoundsFired);

        weapon.ReturnToStorage();

        var log = OperationLog.Create("Return", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been returned to storage with {weapon.Condition} condition", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task SendToMaintenanceAsync(SendWeaponToMaintenanceRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.WeaponId)
            ?? throw new KeyNotFoundException($"Weapon with ID {request.WeaponId} not found.");

        weapon.SendToMaintenance();

        var log = OperationLog.Create("Maintenance", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been sent to maintenance", weapon.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReportMissingAsync(ReportWeaponMissingRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(request.WeaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {request.WeaponId} not found.");

        weapon.MarkAsMissing();

        var log = OperationLog.Create("Missing", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber}\n - Has been marked as {weapon.Status}", weapon.Id);
        _unitOfWork.Logs.Add(log);


        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<WeaponResponse?> GetWeaponByIdAsync(Guid weaponId, CancellationToken cancellationToken)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");

        return MapToResponse(weapon);
    }

    public async Task<List<WeaponResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var weapons = await _unitOfWork.Weapons.GetAllAsync(cancellationToken);

        return weapons.Select(MapToResponse).ToList();
    }

    private static WeaponResponse MapToResponse(Weapon w)
    {
        return new WeaponResponse(
            w.Id,
            w.Codename,
            w.SerialNumber,
            w.Caliber,
            w.Type.ToString(),
            w.Status.ToString(),
            w.Condition,
            w.IssuedToSoldierId
        );
    }
}