using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Unifiedpost.LogReporter.BusinessLogic.Contracts;
using Unifiedpost.LogReporter.BusinessLogic.Models;

namespace Unifiedpost.LogReporter.BusinessLogic.Implementations;

public class CsvHandler : IFileHandler
{
    public async Task WriteFile(IEnumerable<LogEntry?> output, string destinationFilePath, CancellationToken cancellationToken)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "|",
            NewLine = "\n"
        };

        var reader = output.Where(x => x is not null);
        using var writer = new StreamWriter(destinationFilePath, false);
        using var csv = new CsvWriter(writer, config);
        await csv.WriteRecordsAsync(reader, cancellationToken);
    }
}
