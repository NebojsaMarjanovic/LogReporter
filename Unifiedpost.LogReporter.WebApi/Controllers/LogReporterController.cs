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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
