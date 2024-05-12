namespace Unifiedpost.LogReporter.BusinessLogic.Models
{
    public record LogRequestDto(string ServiceName, IEnumerable<string> LogFiles, string Keyword);
}
