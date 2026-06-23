using BG.App.DTOs.OperationLogs;

namespace BG.App.Interfaces;

public interface ILogService
{
    Task<LogResponse> GetLogByIdAsync(Guid logId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogResponse>> GetRecentLogsAsync(CancellationToken cancellationToken = default, int count = 15);
}