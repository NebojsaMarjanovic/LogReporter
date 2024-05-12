using System.Text.RegularExpressions;
using System.Threading.Channels;
using Unifiedpost.LogReporter.BusinessLogic.Contracts;
using Unifiedpost.LogReporter.BusinessLogic.Models;

namespace Unifiedpost.LogReporter.BusinessLogic.Implementations;

public class LogProcessor : ILogProcessor
{
    private static readonly RegexOptions _regexOptions = RegexOptions.Compiled;
    private static readonly Lazy<Regex> _logLineMatcher = new(() => new Regex(@"\[(.*?)\]\s(.*)$", _regexOptions));

    public async Task<IEnumerable<LogEntry>> ProcessLogs(string serviceName, IEnumerable<string> logFiles, string keyword, CancellationToken cancellationToken)
    {
        var channel = Channel.CreateUnbounded<LogEntry>();

        var logEntries = new List<LogEntry>();
        var producers = new List<Task>();

        foreach(var logFile in logFiles)
        {
            var logFileName = logFile.Substring(logFile.LastIndexOf('\\')+1);

            producers.Add(Task.Run(async () =>
            {
                foreach (var line in File.ReadLines(logFile))
                {
                    Match match = _logLineMatcher.Value.Match(line);

                    if (match.Success && line.Contains(keyword))
                    {
                        
                        await channel.Writer.WriteAsync(new LogEntry(serviceName, logFileName, keyword, match.Groups[2].Value));
                    }
                }
            }));
        }

        var consumer = Task.Run(async () =>
        {
            await foreach (var logEntry in channel.Reader.ReadAllAsync())
            {
                logEntries.Add(logEntry);
            }
        });

        await Task.WhenAll(producers);
        channel.Writer.Complete();
        await consumer;

        return logEntries.OrderBy(x => x.ServiceName).ThenBy(x => x.LogFileName);
    }
}
