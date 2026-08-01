using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/// <summary>
/// Controlador para la gestión de gastos de Electrónicos.
/// </summary>
[ApiController]
[Route("api/v1/electronics")]
[Authorize]
public class ElectronicsController : ControllerBase
{
    private readonly IElectronicsService _electronicsService;
    private readonly ILogger<ElectronicsController> _logger;

    public ElectronicsController(IElectronicsService electronicsService, ILogger<ElectronicsController> logger)
    {
        _electronicsService = electronicsService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Electronics electronics)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        electronics.UserId = userId;

        var created = await _electronicsService.CreateElectronics(electronics);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Electronics electronics)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        electronics.Id = id;
        electronics.UserId = userId;

        try
        {
            var updated = await _electronicsService.UpdateElectronics(id, electronics, userId);
            return Ok(updated);
    }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Electronics {ElectronicsId} not found for user {UserId}", id, userId);
            return NotFound(new { message = ex.Message });
    }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();

        var deleted = await _electronicsService.DeleteElectronicsById(id, userId);
        if (!deleted)
        {
            _logger.LogWarning("Electronics {ElectronicsId} not found for deletion by user {UserId}", id, userId);
            return NotFound(new { message = $"Electronics with id '{id}' not found or does not belong to the user." });
    }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var electronics = await _electronicsService.GetElectronicsById(id, GetUserId());
        if (electronics is null)
            return NotFound(new { message = $"Electronics with id '{id}' not found." });

        return Ok(electronics);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchBy = null,
        [FromQuery] string? searchString = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortAscending = true,
        [FromQuery] string? period = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        if (!string.IsNullOrWhiteSpace(period))
        {
            var now = DateTime.UtcNow;
            (startDate, endDate) = period.ToLowerInvariant() switch
            {
                "week" => (now.AddDays(-7), now),
                "month" => (now.AddMonths(-1), now),
                "3months" => (now.AddMonths(-3), now),
                _ => (startDate, endDate)
            };
    }

        var result = await _electronicsService.GetPaginatedElectronics(
            GetUserId(),
            pageIndex,
            pageSize,
            searchBy,
            searchString,
            sortBy,
            sortAscending ? SortOrderEnum.Ascending : SortOrderEnum.Descending,
            startDate,
            endDate);

        return Ok(result);
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}