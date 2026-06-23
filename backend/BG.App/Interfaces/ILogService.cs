

using BG.App.DTOs;

namespace BG.App.Interfaces;

public interface ILogService
{
    Task<LogResponse?> GetLogByIdAsync(Guid logId, CancellationToken cancellationToken = default);
    Task<List<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default);
    Task<List<LogResponse>> GetRecentLogsAsync(CancellationToken cancellationToken = default, int count = 15);
}