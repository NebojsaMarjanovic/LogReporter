using Unifiedpost.LogReporter.BusinessLogic.Models;

namespace Unifiedpost.LogReporter.BusinessLogic.Contracts;

public interface ILogProcessor
{
    Task<IEnumerable<LogEntry>> ProcessLogs(string serviceName, IEnumerable<string> logFiles, string keyword, CancellationToken cancellationToken);
}
