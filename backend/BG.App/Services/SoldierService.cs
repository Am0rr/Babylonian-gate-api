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

    public async Task UpdateAsync(UpdateSoldierRequest request, CancellationToken cancellationToken)
    {
        Validate(request);

        var soldier = await _unitOfWork.Soldiers.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Soldier with ID {request.Id} not found.");

        var rank = Enum.Parse<SoldierRank>(request.Rank, ignoreCase: true);

        bool hasChanges = false;
        var logDetails = new List<string>();

        if (soldier.FirstName != request.FirstName || soldier.LastName != request.LastName)
        {
            string oldFirstName = soldier.FirstName;
            string oldLastName = soldier.LastName;
            soldier.UpdateName(request.FirstName, request.LastName);
            logDetails.Add($"First and Last name: '{oldFirstName}' '{oldLastName}' -> '{request.FirstName}' '{request.LastName}'");
            hasChanges = true;
        }

        if (soldier.Rank != rank)
        {
            string oldRank = soldier.Rank.ToString();
            soldier.UpdateRank(rank);
            logDetails.Add($"Rank: '{oldRank}' -> '{request.Rank}'");
            hasChanges = true;
        }

        if (!hasChanges)
        {
            return;
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