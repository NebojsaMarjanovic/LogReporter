using Microsoft.AspNetCore.Mvc;
using System.Net;
using Unifiedpost.LogReporter.WebApi.Contracts;
using Unifiedpost.LogReporter.WebApi.Models;

namespace Unifiedpost.LogReporter.WebApi.Controllers;

[Route("api")]
[ApiController]
public class LogReporterController : ControllerBase
{
    private readonly ILogReporterService _logReporterService;
    private readonly ILogger<LogReporterController> _logger;

    public LogReporterController([FromKeyedServices("csv")] ILogReporterService logReporterService, ILogger<LogReporterController> logger)
    {
        _logReporterService = logReporterService;
        _logger = logger;
    }
    /// <summary>
    /// Generates a log report in a .csv file based on keyword search.
    /// </summary>
    /// <param name="logReportRequest">
    /// Request parameters:
    /// 
    ///     {
    ///         [Required] "serviceName": "string",
    ///         [Required] "searchType": 0,     //{0 - All, 1 - Specific, 2-Combination}
    ///         "specificLogs": ["string", "string"],
    ///         [Required] "keyword": "string",
    ///         [Required] "from": "2024-05-12T17:57:27.447Z",
    ///         [Required] "to": "2024-05-12T17:57:27.447Z"
    ///     }
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Sample for fetching logs from 3 different mock services by different keywords per service and different search types
    ///
    ///     POST /csvReport
    ///     {
    ///         "requestParameters": [
    ///             {
    ///                 "serviceName": "service1",
    ///                 "searchType": 0,
    ///                 "keyword": "Information",
    ///                 "from": "2024-03-12T17:57:27.447Z",
    ///                 "to": "2024-06-12T17:57:27.447Z"
    ///             },
    ///             {
    ///                 "serviceName": "service2",
    ///                 "searchType": 1,
    ///                 "specificLogs": ["log1.txt"],
    ///                 "keyword": "Error",
    ///                 "from": "2024-04-20T17:57:27.447Z",
    ///                 "to": "2024-06-12T17:57:27.447Z"
    ///             },
    ///             {
    ///                 "serviceName": "service3",
    ///                 "searchType": 2,
    ///                 "specificLogs": ["log1.txt", "log2.txt"],
    ///                 "keyword": "user",
    ///                 "from": "2024-01-05T17:57:27.447Z",
    ///                 "to": "2024-06-12T17:57:27.447Z"
    ///             }
    ///         ]
    ///     }
    /// </remarks>
    /// <response code="500">Indicates that some transient error happened on the server side.</response>

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Route("csvReport")]
    public async Task<IActionResult> GenerateCsvLogReport(LogReportRequest logReportRequest, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing log report request...");
            await _logReporterService.GenerateReport(logReportRequest, cancellationToken);
            return Ok("Successfuly generated .csv file.");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
        finally
        {
            _logger.LogInformation("End request processing.");
        }

    }
}
