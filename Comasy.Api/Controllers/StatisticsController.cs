using Comasy.Api.Models;
using Comasy.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Comasy.Api.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController(IStatisticsService statisticsService) : ControllerBase
{
    /// <summary>
    /// Registrerar en sidvisning för angiven sida.
    /// </summary>
    /// <param name="request">Id för sidan som visades.</param>
    /// <response code="204">Visningen registrerades.</response>
    /// <response code="400">PageId saknas eller är inte ett positivt tal.</response>
    [HttpPost("views")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterView([FromBody] RegisterViewRequest request)
    {
        try
        {
            await statisticsService.RegisterViewAsync(request.PageId);
            return NoContent();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Hämtar antalet visningar för en enskild sida.
    /// </summary>
    /// <param name="pageId">Sidans id i publiceringssystemet.</param>
    /// <response code="200">Antalet visningar. Noll om sidan aldrig visats.</response>
    /// <response code="400">PageId är inte ett positivt tal.</response>
    [HttpGet("views/{pageId:int}")]
    [ProducesResponseType(typeof(ViewCountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ViewCountResponse>> GetViewCount(int pageId)
    {
        try
        {
            var count = await statisticsService.GetViewCountAsync(pageId);
            return Ok(new ViewCountResponse(pageId, count));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Hämtar antalet visningar för samtliga sidor som har minst en visning.
    /// </summary>
    /// <response code="200">En lista med sidid och antal visningar.</response>
    [HttpGet("views")]
    [ProducesResponseType(typeof(IEnumerable<ViewCountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ViewCountResponse>>> GetViewCounts()
    {
        var counts = await statisticsService.GetViewCountsAsync();

        var response = counts
            .Select(pair => new ViewCountResponse(pair.Key, pair.Value))
            .OrderByDescending(item => item.Count)
            .ToList();

        return Ok(response);
    }

    /// <summary>
    /// Hämtar totalt antal sidvisningar under de senaste dygnen.
    /// </summary>
    /// <param name="days">Antal dygn bakåt att räkna. Standard är 7.</param>
    /// <response code="200">Antalet visningar under perioden.</response>
    /// <response code="400">Antal dagar är inte ett positivt tal.</response>
    [HttpGet("views/recent")]
    [ProducesResponseType(typeof(RecentViewsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RecentViewsResponse>> GetRecentViews([FromQuery] int days = 7)
    {
        try
        {
            var count = await statisticsService.GetViewsLastDaysAsync(days);
            return Ok(new RecentViewsResponse(days, count));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}