using System.ComponentModel.DataAnnotations;

namespace Unifiedpost.LogReporter.WebApi.Models;


public class LogReportRequest
{
    [Required]
    public List<LogReportRequestParameters> RequestParameters { get; set; }
}

public record LogReportRequestParameters ([Required] string ServiceName,[Required] SearchType SearchType, string[]? SpecificLogs,[Required] string Keyword, [Required] DateTime From, [Required] DateTime To);

public enum SearchType
{
    All,
    Specific,
    Combination
}
