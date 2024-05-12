using System.ComponentModel.DataAnnotations;

namespace Unifiedpost.LogReporter.WebApi.Configurations;

public class LogFileConfiguration
{
    public Dictionary<string, LogServiceConfiguration> LogFiles { get; set; }
}

//FIX - add validate on start
public class LogServiceConfiguration
{
    [Required]
    public string Path { get; set; } = null!;
}
