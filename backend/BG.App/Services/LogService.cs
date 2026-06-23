using BG.App.DTOs.OperationLogs;
using BG.App.Interfaces;
using BG.Domain.Entities;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;

    public LogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LogResponse?> GetLogByIdAsync(Guid logId, CancellationToken cancellationToken)
    {
        var log = await _unitOfWork.Logs.GetByIdAsync(logId, cancellationToken)
            ?? throw new KeyNotFoundException($"Log with ID {logId} not found.");

        return MapToResponse(log);
    }

    public async Task<List<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId, CancellationToken cancellationToken)
    {
        var logs = await _unitOfWork.Logs.GetByEntityIdAsync(entityId, cancellationToken);

        return logs.OrderByDescending(x => x.CreatedAt).Select(MapToResponse).ToList();
    }

    public async Task<List<LogResponse>> GetRecentLogsAsync(CancellationToken cancellationToken, int count = 15)
    {
        var logs = await _unitOfWork.Logs.GetRecentAsync(count, cancellationToken);

        return logs.Select(MapToResponse).ToList();
    }

    private static LogResponse MapToResponse(OperationLog o)
    {
        return new LogResponse(
            o.Id,
            o.Action,
            o.Details,
            o.CreatedAt,
            o.RelatedEntityId,
            o.OperatorId
        );
    }
}