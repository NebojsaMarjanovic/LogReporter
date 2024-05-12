using Microsoft.Extensions.Options;
using Unifiedpost.LogReporter.BusinessLogic.Contracts;
using Unifiedpost.LogReporter.BusinessLogic.Models;
using Unifiedpost.LogReporter.WebApi.Configurations;
using Unifiedpost.LogReporter.WebApi.Contracts;
using Unifiedpost.LogReporter.WebApi.Models;

namespace Unifiedpost.LogReporter.WebApi.Implementations
{
    public class CsvLogReporterService : ILogReporterService
    {
        private readonly IFileHandler _fileHandler;
        private readonly CsvConfiguration _csvConfiguration;
        private readonly ILogProcessor _logProcessor;
        private readonly LogFileConfiguration _logFileConfiguration;

        public CsvLogReporterService([FromKeyedServices("csv")] IFileHandler fileHandler, ILogProcessor logProcessor,
            IOptions<LogFileConfiguration> logFileConfiguration, IOptions<CsvConfiguration> csvConfiguration)
        {
            _fileHandler = fileHandler;
            _logProcessor = logProcessor;
            _logFileConfiguration = logFileConfiguration.Value;
            _csvConfiguration = csvConfiguration.Value;
        }

        public async Task GenerateReport(LogReportRequest logReportRequest, CancellationToken cancellationToken)
        {
            var logEntries = new List<LogEntry>();
            var logFiles = new List<string>();



            foreach (var request in logReportRequest.RequestParameters)
            {

                if (request.SearchType is not SearchType.All && request.SpecificLogs is not null)
                {
                    foreach (var specificLog in request.SpecificLogs)
                    {
                        logFiles.AddRange(Directory.GetFiles(_logFileConfiguration.LogFiles[request.ServiceName].Path, specificLog)
                            .Where(file =>
                            {
                                DateTime creationDate = File.GetCreationTime(file);
                                return creationDate >= request.From && creationDate <= request.To;
                            }));
                    }
                }
                else
                {
                    logFiles.AddRange(Directory.GetFiles(_logFileConfiguration.LogFiles[request.ServiceName].Path)
                        .Where(file =>
                        {
                            DateTime creationDate = File.GetCreationTime(file);
                            return creationDate >= request.From && creationDate <= request.To;
                        }));
                }

                logEntries.AddRange(await _logProcessor.ProcessLogs(request.ServiceName, logFiles, request.Keyword, cancellationToken));
            }
            await _fileHandler.WriteFile(logEntries, $"{_csvConfiguration.DestinationPath}\\report_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv", cancellationToken);
        }
    }
}
