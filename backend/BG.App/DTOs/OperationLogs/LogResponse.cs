namespace BG.App.DTOs.OperationLogs;

public record LogResponse(
    Guid Id,
    string Action,
    string Details,
    DateTime CreatedAt,
    Guid? RelatedEntityId,
    Guid? OperatorId
);