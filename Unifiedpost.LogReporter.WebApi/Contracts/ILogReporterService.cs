using Unifiedpost.LogReporter.WebApi.Models;

namespace Unifiedpost.LogReporter.WebApi.Contracts;

public interface ILogReporterService
{
    Task GenerateReport(LogReportRequest logReportRequest, CancellationToken cancellationToken);
}
