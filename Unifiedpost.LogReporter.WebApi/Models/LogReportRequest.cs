namespace Unifiedpost.LogReporter.WebApi.Models;


public class LogReportRequest
{
    public List<LogReportRequestParameters> RequestParameters { get; set; }
}

public record LogReportRequestParameters (string ServiceName, SearchType SearchType, string[]? SpecificLogs, string Keyword, DateTime From, DateTime To);

public enum SearchType
{
    All,
    Specific,
    Combination
}
