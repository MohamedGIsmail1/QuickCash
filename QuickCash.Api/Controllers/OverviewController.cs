using Microsoft.AspNetCore.Mvc;
using QuickCash.Api.Dtos.Overview;
using QuickCash.Api.Services.Interfaces;

namespace QuickCash.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OverviewController : ControllerBase
{
    private readonly IOverviewService _service;

    public OverviewController(IOverviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<MonthlyOverviewDto>> GetMonthly([FromQuery] int year, [FromQuery] int month)
    {
        try
        {
            var result = await _service.GetMonthlyAsync(year, month);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
