using BG.App.DTOs.Soldiers;
using BG.App.Interfaces;
using BG.Domain.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace BG.App.Services;

public class SoldierService : BaseService, ISoldierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SoldierService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateSoldierRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var rank = Enum.Parse<SoldierRank>(request.Rank, ignoreCase: true);

        var soldier = new Soldier(
            request.FirstName,
            request.LastName,
            rank
        );

        _unitOfWork.Soldiers.Add(soldier);

        var log = OperationLog.Create("Create", $"Recruited soldier {soldier.LastName}", soldier.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return soldier.Id;
    }

    public async Task UpdateAsync(Guid soldierId, UpdateSoldierRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {soldierId} not found.");

        var logDetails = new List<string>();

        if (request.FirstName != null)
        {
            string oldFirstName = soldier.FirstName;
            soldier.ChangeFirstName(request.FirstName);
            logDetails.Add($"First name: '{oldFirstName} -> '{request.FirstName}'");
        }

        if (request.LastName != null)
        {
            string oldLastName = soldier.LastName;
            soldier.ChangeFirstName(request.LastName);
            logDetails.Add($"Last name: '{oldLastName} -> '{request.LastName}'");
        }

        if (request.Rank != null)
        {
            string oldRank = soldier.Rank.ToString();

            var rank = Enum.Parse<SoldierRank>(request.Rank, ignoreCase: true);
            soldier.ChangeRank(rank);
            logDetails.Add($"Rank: '{oldRank}' -> '{request.Rank}'");
        }

        _unitOfWork.Soldiers.Update(soldier);

        var log = OperationLog.Create("Update", $"Updated soldier {soldier.LastName}. Changes: {string.Join(", ", logDetails)}", soldier.Id);
        _unitOfWork.Logs.Add(log);


        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid soldierId, CancellationToken cancellationToken)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {soldierId} not found.");

        var hasWeapon = await _unitOfWork.Weapons.Query().AnyAsync(w => w.IssuedToSoldierId == soldierId, cancellationToken);

        if (hasWeapon)
            throw new InvalidOperationException("Cannot delete soldier because they still have assigned weapons. Return weapons to storage first.");

        _unitOfWork.Soldiers.Delete(soldier);

        var log = OperationLog.Create("Delete", $"Discharged soldier {soldier.LastName}", soldier.Id);
        _unitOfWork.Logs.Add(log);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<SoldierResponse> GetSoldierByIdAsync(Guid soldierId, CancellationToken cancellationToken)
    {
        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(soldierId, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {soldierId} not found.");

        return _mapper.Map<SoldierResponse>(soldier);
    }

    public async Task<IEnumerable<SoldierResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var soldiers = await _unitOfWork.Soldiers.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<SoldierResponse>>(soldiers);
    }
}