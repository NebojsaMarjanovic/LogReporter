namespace Unifiedpost.LogReporter.BusinessLogic.Models;

public record LogEntry(string? ServiceName, string? LogFileName, string? SearchKeyword, string? Record);

