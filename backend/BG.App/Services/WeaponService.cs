using AutoMapper;
using BG.App.DTOs.Weapons;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Enums;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class WeaponService : BaseService, IWeaponService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WeaponService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WeaponResponse> CreateAsync(CreateWeaponRequest request, CancellationToken cancellationToken)
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

        return _mapper.Map<WeaponResponse>(weapon);
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

    public async Task UpdateDetailsAsync(Guid weaponId, UpdateWeaponDetailsRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");

        var logDetails = new List<string>();

        if (request.Codename != null)
        {
            string oldCodeName = weapon.Codename;

            weapon.ChangeCodeName(request.Codename);

            logDetails.Add($"Codename: '{oldCodeName}' -> '{request.Codename}'");
        }

        if (request.SerialNumber != null)
        {
            string oldSerialNumber = weapon.SerialNumber;

            weapon.ChangeSerialNumber(request.SerialNumber);

            logDetails.Add($"Serial Number: '{oldSerialNumber}' -> '{request.SerialNumber}'");
        }

        if (request.Caliber != null)
        {
            string oldCaliber = weapon.Caliber;

            weapon.ChangeCaliber(request.Caliber);

            logDetails.Add($"Caliber: '{oldCaliber}' -> '{request.Caliber}'");
        }

        if (request.Type != null)
        {
            var oldType = weapon.Type;

            var type = Enum.Parse<WeaponType>(request.Type, ignoreCase: true);

            weapon.ChangeType(type);

            logDetails.Add($"Type: '{oldType}' -> '{type}'");
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

        // var log = OperationLog.Create("Issue", $"Weapon {weapon.Codename}, with SN {weapon.SerialNumber} \n - Issued to {soldier.LastName} {soldier.FirstName} (ID: {soldier.Id})", weapon.Id);
        // _unitOfWork.Logs.Add(log);

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

    public async Task<WeaponResponse> GetWeaponByIdAsync(Guid weaponId, CancellationToken cancellationToken)
    {
        var weapon = await _unitOfWork.Weapons.GetByIdAsync(weaponId, cancellationToken)
            ?? throw new KeyNotFoundException($"Weapon with ID {weaponId} not found.");

        return _mapper.Map<WeaponResponse>(weapon);
    }

    public async Task<IEnumerable<WeaponResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var weapons = await _unitOfWork.Weapons.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<WeaponResponse>>(weapons);
    }
}