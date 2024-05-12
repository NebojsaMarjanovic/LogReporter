using System.Threading.Channels;
using Unifiedpost.LogReporter.BusinessLogic.Contracts;
using Unifiedpost.LogReporter.BusinessLogic.Models;

namespace Unifiedpost.LogReporter.BusinessLogic.Implementations;

public class LogProcessor : ILogProcessor
{
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
                    if (line.Contains(keyword))
                    {
                        //should fix this
                        await channel.Writer.WriteAsync(new LogEntry(serviceName, logFileName, keyword, line.Substring(line.LastIndexOf(']')+2)));
                    }
                }
            }));
        }

        var consumer = Task.Run(async () =>
        {
            await Parallel.ForEachAsync(channel.Reader.ReadAllAsync(), async (logEntry, _) =>
            {
                logEntries.Add(logEntry);
            });
        });

        await Task.WhenAll(producers);
        channel.Writer.Complete();
        await consumer;

        return logEntries.OrderBy(x => x.ServiceName).ThenBy(x => x.LogFileName);
    }
}
