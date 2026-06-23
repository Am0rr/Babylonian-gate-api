using AutoMapper;
using BG.App.DTOs.OperationLogs;
using BG.App.Interfaces;
using BG.Domain.Interfaces;

namespace BG.App.Services;

public class LogService : ILogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LogResponse> GetLogByIdAsync(Guid logId, CancellationToken cancellationToken)
    {
        var log = await _unitOfWork.Logs.GetByIdAsync(logId, cancellationToken)
            ?? throw new KeyNotFoundException($"Log with ID {logId} not found.");

        return _mapper.Map<LogResponse>(log);
    }

    public async Task<IEnumerable<LogResponse>> GetHistoryByEntityIdAsync(Guid entityId, CancellationToken cancellationToken)
    {
        var logs = await _unitOfWork.Logs.GetByEntityIdAsync(entityId, cancellationToken);

        return _mapper.Map<IEnumerable<LogResponse>>(logs).OrderByDescending(x => x.CreatedAt);
    }

    public async Task<IEnumerable<LogResponse>> GetRecentLogsAsync(CancellationToken cancellationToken, int count = 15)
    {
        var logs = await _unitOfWork.Logs.GetRecentAsync(count, cancellationToken);

        return _mapper.Map<IEnumerable<LogResponse>>(logs);
    }
}