using Unifiedpost.LogReporter.BusinessLogic.Models;

namespace Unifiedpost.LogReporter.BusinessLogic.Contracts;

public interface IFileHandler
{
    Task WriteFile(IEnumerable<LogEntry?> output, string destinationFilePath, CancellationToken cancellationToken);
}
